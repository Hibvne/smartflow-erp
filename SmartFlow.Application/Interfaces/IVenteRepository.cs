using SmartFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.Interfaces
{
    public interface IVenteRepository
    {
        Task<(List<Vente> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize);
        Task<Vente> GetByIdAsync(int id);
        Task AddAsync(Vente vente);
        Task SaveChangesAsync();
    }
}
