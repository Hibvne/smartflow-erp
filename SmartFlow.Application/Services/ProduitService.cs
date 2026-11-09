using SmartFlow.Application.DTOs.Clients;
using SmartFlow.Application.DTOs.Produits;
using SmartFlow.Application.Interfaces;
using SmartFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.Services
{
    public class ProduitService
    {
        private readonly IProduitRepository _produitRepository;
        private readonly IAuditLogRepository _auditLogRepository;
        private readonly ICategorieRepository _categorieRepository;

        public ProduitService(IProduitRepository produitRepository, IAuditLogRepository auditLogRepository, ICategorieRepository categorieRepository )
        {
            _produitRepository = produitRepository;
            _auditLogRepository = auditLogRepository;
            _categorieRepository = categorieRepository;
        }

        // Petite méthode privée pour éviter de répéter la conversion Entité -> DTO partout
        private ProduitDto ToDto(Produit p) => new ProduitDto
        {
            Id = p.Id,
            Nom = p.Nom,
            Reference = p.Reference,
            PrixAchat = p.PrixAchat,
            PrixVente = p.PrixVente,
            QuantiteStock = p.QuantiteStock,
            SeuilAlerte = p.SeuilAlerte,
            CategorieId = p.CategorieId,
            CategorieNom = p.Categorie?.Nom // "?." = évite un crash si jamais Categorie est null
        };

        public async Task<PagedResultDto<ProduitDto>> GetPagedAsync(int pageNumber, int pageSize, string search)
        {
            var (items, totalCount) = await _produitRepository.GetPagedAsync(pageNumber, pageSize, search);

            return new PagedResultDto<ProduitDto>
            {
                Items = items.Select(ToDto).ToList(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ProduitDto> GetByIdAsync(int id)
        {
            var produit = await _produitRepository.GetByIdAsync(id);
            if (produit == null) throw new Exception("Produit introuvable.");
            return ToDto(produit);
        }

        public async Task<string> CreateAsync(CreateProduitDto dto)
        {
            var categorie = await _categorieRepository.GetByIdAsync(dto.CategorieId);
            if (categorie == null) throw new Exception("Catégorie introuvable.");

            var countCategorie = await _produitRepository.GetCountByCategorieAsync(dto.CategorieId);
            var reference = $"{categorie.Prefixe}-{(countCategorie + 1):D4}"; // ex: VET-0007

            var produit = new Produit
            {
                Nom = dto.Nom,
                Reference = reference,
                PrixAchat = dto.PrixAchat,
                PrixVente = dto.PrixVente,
                QuantiteStock = dto.QuantiteStock,
                SeuilAlerte = dto.SeuilAlerte,
                CategorieId = dto.CategorieId
            };

            await _produitRepository.AddAsync(produit);
            await _produitRepository.SaveChangesAsync();
            return "Produit créé avec succès.";
        } 

        public async Task<string> UpdateAsync(int id, UpdateProduitDto dto)
        {
            var produit = await _produitRepository.GetByIdAsync(id);
            if (produit == null) throw new Exception("Produit introuvable.");

            produit.Nom = dto.Nom;
            produit.Reference = dto.Reference;
            produit.PrixAchat = dto.PrixAchat;
            produit.PrixVente = dto.PrixVente;
            produit.QuantiteStock = dto.QuantiteStock;
            produit.SeuilAlerte = dto.SeuilAlerte;
            produit.CategorieId = dto.CategorieId;

            _produitRepository.Update(produit);
            await _produitRepository.SaveChangesAsync();
            return "Produit mis à jour avec succès.";
        }

        public async Task<string> DeleteAsync(int id)
        {
            var produit = await _produitRepository.GetByIdAsync(id);
            if (produit == null) throw new Exception("Produit introuvable.");

            _produitRepository.Delete(produit);
            await _produitRepository.SaveChangesAsync();
            return "Produit supprimé avec succès.";
        }

        // utilisateurId vient du token JWT connecté (le contrôleur nous le passera)
        public async Task<string> EntreeStockAsync(int produitId, MouvementStockDto dto, int utilisateurId)
        {
            var produit = await _produitRepository.GetByIdAsync(produitId);
            if (produit == null) throw new Exception("Produit introuvable.");

            produit.QuantiteStock += dto.Quantite; // entrée = on ajoute au stock

            _produitRepository.Update(produit);

            // Traçabilité : on garde une trace de cette action dans AuditLog
            await _auditLogRepository.AddAsync(new AuditLog
            {
                Action = $"Entrée stock : +{dto.Quantite} sur '{produit.Nom}' ({dto.Motif})",
                UtilisateurId = utilisateurId
            });

            await _produitRepository.SaveChangesAsync();
            return "Stock mis à jour (entrée).";
        }

        public async Task<string> SortieStockAsync(int produitId, MouvementStockDto dto, int utilisateurId)
        {
            var produit = await _produitRepository.GetByIdAsync(produitId);
            if (produit == null) throw new Exception("Produit introuvable.");

            // Vérification métier importante : on ne peut pas sortir plus que ce qu'on a
            if (produit.QuantiteStock < dto.Quantite)
                throw new Exception($"Stock insuffisant. Stock actuel : {produit.QuantiteStock}.");

            produit.QuantiteStock -= dto.Quantite;

            _produitRepository.Update(produit);

            await _auditLogRepository.AddAsync(new AuditLog
            {
                Action = $"Sortie stock : -{dto.Quantite} sur '{produit.Nom}' ({dto.Motif})",
                UtilisateurId = utilisateurId
            });

            await _produitRepository.SaveChangesAsync();
            return "Stock mis à jour (sortie).";
        }


    }
}
