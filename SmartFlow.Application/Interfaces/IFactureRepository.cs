using SmartFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.Interfaces
{
    public interface IFactureRepository
    {
        Task<(List<Facture> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<Facture> GetByIdAsync(int id);
        Task<int> GetCountAsync(); // utile pour générer un numéro de facture séquentiel
        Task AddAsync(Facture facture);
        Task SaveChangesAsync();
    }
}
