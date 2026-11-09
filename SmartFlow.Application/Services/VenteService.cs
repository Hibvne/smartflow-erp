using SmartFlow.Application.DTOs.Clients;
using SmartFlow.Application.DTOs.Ventes;
using SmartFlow.Application.Interfaces;
using SmartFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.Services
{
    public class VenteService
    {
        private readonly IVenteRepository _venteRepository;
        private readonly IProduitRepository _produitRepository;
        private readonly IFactureRepository _factureRepository;
        private readonly IAuditLogRepository _auditLogRepository;

        public VenteService(
            IVenteRepository venteRepository,
            IProduitRepository produitRepository,
            IFactureRepository factureRepository,
            IAuditLogRepository auditLogRepository)
        {
            _venteRepository = venteRepository;
            _produitRepository = produitRepository;
            _factureRepository = factureRepository;
            _auditLogRepository = auditLogRepository;
        }

        private VenteDto ToDto(Vente v) => new VenteDto
        {
            Id = v.Id,
            DateVente = v.DateVente,
            MontantTotal = v.MontantTotal,
            ClientId = v.ClientId,
            ClientNom = v.Client?.Nom,
            VendeurNom = v.Utilisateur?.NomComplet,
            NumeroFacture = v.Id.ToString(), // sera remplacé par le vrai numéro via GetByIdAsync si besoin
            Lignes = v.LignesVente?.Select(l => new LigneVenteDto
            {
                ProduitId = l.ProduitId,
                ProduitNom = l.Produit?.Nom,
                Quantite = l.Quantite,
                PrixUnitaire = l.PrixUnitaire
            }).ToList()
        };

        public async Task<PagedResultDto<VenteDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var (items, totalCount) = await _venteRepository.GetPagedAsync(pageNumber, pageSize);
            return new PagedResultDto<VenteDto>
            {
                Items = items.Select(ToDto).ToList(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<VenteDto> GetByIdAsync(int id)
        {
            var vente = await _venteRepository.GetByIdAsync(id);
            if (vente == null) throw new Exception("Vente introuvable.");
            return ToDto(vente);
        }

        // Le cœur du module : création d'une vente complète, transactionnelle
        public async Task<VenteDto> CreateAsync(CreateVenteDto dto, int utilisateurId)
        {
            if (dto.Lignes == null || !dto.Lignes.Any())
                throw new Exception("Une vente doit contenir au moins une ligne.");

            // --- ÉTAPE 1 : Validation complète AVANT toute modification ---
            // On charge tous les produits et vérifie le stock, sans encore rien modifier.
            var produits = new Dictionary<int, Produit>();

            foreach (var ligne in dto.Lignes)
            {
                var produit = await _produitRepository.GetByIdAsync(ligne.ProduitId);
                if (produit == null)
                    throw new Exception($"Produit Id {ligne.ProduitId} introuvable.");

                if (produit.QuantiteStock < ligne.Quantite)
                    throw new Exception($"Stock insuffisant pour '{produit.Nom}' (disponible : {produit.QuantiteStock}).");

                produits[ligne.ProduitId] = produit;
            }

            // --- ÉTAPE 2 : Construction de la Vente + décrément du stock (encore en mémoire, rien sauvegardé) ---
            var vente = new Vente
            {
                ClientId = dto.ClientId,
                UtilisateurId = utilisateurId,
                LignesVente = new List<LigneVente>()
            };

            decimal montantTotal = 0;

            foreach (var ligne in dto.Lignes)
            {
                var produit = produits[ligne.ProduitId];

                var ligneVente = new LigneVente
                {
                    ProduitId = produit.Id,
                    Quantite = ligne.Quantite,
                    PrixUnitaire = produit.PrixVente // on fige le prix au moment de la vente
                };

                vente.LignesVente.Add(ligneVente);
                montantTotal += ligne.Quantite * produit.PrixVente;

                produit.QuantiteStock -= ligne.Quantite; // décrément automatique du stock
                _produitRepository.Update(produit);
            }

            vente.MontantTotal = montantTotal;

            await _venteRepository.AddAsync(vente);

            // --- ÉTAPE 3 : Génération automatique de la Facture liée ---
            var numeroFacture = $"FAC-{(await _factureRepository.GetCountAsync() + 1):D5}"; // ex: FAC-00001

            var facture = new Facture
            {
                NumeroFacture = numeroFacture,
                MontantTotal = montantTotal,
                Statut = "En attente",
                Vente = vente // on lie directement l'objet, EF Core gérera le VenteId automatiquement
            };

            await _factureRepository.AddAsync(facture);

            // --- ÉTAPE 4 : Traçabilité ---
            await _auditLogRepository.AddAsync(new AuditLog
            {
                Action = $"Vente créée : {numeroFacture}, montant {montantTotal} DH",
                UtilisateurId = utilisateurId
            });

            // --- ÉTAPE 5 : Un SEUL SaveChanges, tout est sauvegardé ensemble ---
            await _venteRepository.SaveChangesAsync();

            var venteDto = ToDto(vente);
            venteDto.NumeroFacture = numeroFacture;
            return venteDto;
        }
    }
}
