using SmartFlow.Application.DTOs.Categories;
using SmartFlow.Application.Interfaces;
using SmartFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.Services
{
    public class CategorieService
    {
        private readonly ICategorieRepository _categorieRepository;

        public CategorieService(ICategorieRepository categorieRepository)
        {
            _categorieRepository = categorieRepository;
        }

        public async Task<List<CategorieDto>> GetAllAsync()
        {
            var categories = await _categorieRepository.GetAllAsync();

            // Conversion Entité -> DTO (jamais on ne renvoie l'entité brute au frontend)
            return categories.Select(c => new CategorieDto
            {
                Id = c.Id,
                Nom = c.Nom,
                Description = c.Description,
                Prefixe = c.Prefixe
            }).ToList();
        }

        public async Task<CategorieDto> GetByIdAsync(int id)
        {
            var categorie = await _categorieRepository.GetByIdAsync(id);
            if (categorie == null) throw new Exception("Catégorie introuvable.");

            return new CategorieDto { Id = categorie.Id, Nom = categorie.Nom, Description = categorie.Description, Prefixe = categorie.Prefixe };
        }

        public async Task<string> CreateAsync(CreateCategorieDto dto)
        {
            var categorie = new Categorie
            {
                Nom = dto.Nom,
                Description = dto.Description,
                Prefixe = dto.Prefixe.ToUpper() // toujours en majuscules, quoi que l'utilisateur tape
            };
            await _categorieRepository.AddAsync(categorie);
            await _categorieRepository.SaveChangesAsync();
            return "Catégorie créée avec succès.";
        }

        public async Task<string> UpdateAsync(int id, UpdateCategorieDto dto)
        {
            var categorie = await _categorieRepository.GetByIdAsync(id);
            if (categorie == null) throw new Exception("Catégorie introuvable.");

            categorie.Nom = dto.Nom;
            categorie.Description = dto.Description;
            categorie.Prefixe = dto.Prefixe.ToUpper();

            _categorieRepository.Update(categorie);
            await _categorieRepository.SaveChangesAsync();
            return "Catégorie mise à jour avec succès.";
        }

        public async Task<string> DeleteAsync(int id)
        {
            var categorie = await _categorieRepository.GetByIdAsync(id);
            if (categorie == null) throw new Exception("Catégorie introuvable.");

            _categorieRepository.Delete(categorie);
            await _categorieRepository.SaveChangesAsync();
            return "Catégorie supprimée avec succès.";
        }
    }
}
