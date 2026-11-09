using Microsoft.EntityFrameworkCore;
using SmartFlow.Application.Interfaces;
using SmartFlow.Domain.Entities;
using SmartFlow.Infrastructure.Data;

namespace SmartFlow.Infrastructure.Repositories
{
    public class FactureRepository : IFactureRepository
    {
        private readonly SmartFlowDbContext _context;

        public FactureRepository(SmartFlowDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Facture> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Factures
                .Include(f => f.Vente).ThenInclude(v => v.Client)
                .OrderByDescending(f => f.DateFacture);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Facture> GetByIdAsync(int id)
        {
            return await _context.Factures
                .Include(f => f.Vente).ThenInclude(v => v.Client)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Factures.CountAsync();
        }

        public async Task AddAsync(Facture facture)
        {
            await _context.Factures.AddAsync(facture);
            // Pas de SaveChanges ici : la Facture est sauvegardée en même temps
            // que la Vente dans VenteService, comme un seul bloc cohérent
        }
    }
}