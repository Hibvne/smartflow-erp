using SmartFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.Interfaces
{
    public interface ICategorieRepository
    {
        // Pas de pagination ici volontairement : une liste de catégories reste courte
        // (utile pour remplir un menu déroulant côté frontend, par exemple)
        Task<List<Categorie>> GetAllAsync();
        Task<Categorie> GetByIdAsync(int id);
        Task AddAsync(Categorie categorie);
        void Update(Categorie categorie);
        void Delete(Categorie categorie);
        Task SaveChangesAsync();
    }
}
