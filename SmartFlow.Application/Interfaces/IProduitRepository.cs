using SmartFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.Interfaces
{
    public interface IProduitRepository
    {
        Task<(List<Produit> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string search);
        Task<Produit> GetByIdAsync(int id);
        Task AddAsync(Produit produit);
        void Update(Produit produit);
        void Delete(Produit produit);
        Task SaveChangesAsync();
        Task<int> GetCountAsync();
        Task<int> GetCountByCategorieAsync(int categorieId);
    }
}
