using Microsoft.EntityFrameworkCore;
using SmartFlow.Application.Interfaces;
using SmartFlow.Domain.Entities;
using SmartFlow.Infrastructure.Data;

namespace SmartFlow.Infrastructure.Repositories
{
    public class ClientRepository : IClientRepository
    {
        private readonly SmartFlowDbContext _context;

        public ClientRepository(SmartFlowDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Client> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string search)
        {
            var query = _context.Clients.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(c =>
                    c.Nom.Contains(search) ||
                    c.Email.Contains(search));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderByDescending(c => c.DateCreation)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Client> GetByIdAsync(int id)
        {
            return await _context.Clients.FindAsync(id);
        }

        public async Task AddAsync(Client client)
        {
            await _context.Clients.AddAsync(client);
        }

        public void Update(Client client)
        {
            _context.Clients.Update(client);
        }

        public void Delete(Client client)
        {
            _context.Clients.Remove(client);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}