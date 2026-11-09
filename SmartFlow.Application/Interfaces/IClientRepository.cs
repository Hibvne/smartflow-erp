using SmartFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.Interfaces
{
    public interface IClientRepository
    {
        Task<(List<Client> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string search);
        Task<Client> GetByIdAsync(int id);
        Task AddAsync(Client client);
        void Update(Client client);
        void Delete(Client client);
        Task SaveChangesAsync();
    }
}
