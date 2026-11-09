using Microsoft.EntityFrameworkCore;
using SmartFlow.Application.Interfaces;
using SmartFlow.Domain.Entities;
using SmartFlow.Infrastructure.Data;

namespace SmartFlow.Infrastructure.Repositories
{
    public class VenteRepository : IVenteRepository
    {
        private readonly SmartFlowDbContext _context;

        public VenteRepository(SmartFlowDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Vente> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize)
        {
            var query = _context.Ventes
                .Include(v => v.Client)
                .Include(v => v.Utilisateur)
                .Include(v => v.LignesVente).ThenInclude(l => l.Produit)
                .OrderByDescending(v => v.DateVente);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Vente> GetByIdAsync(int id)
        {
            // .ThenInclude() charge une relation "de 2e niveau" : Vente -> LignesVente -> Produit
            return await _context.Ventes
                .Include(v => v.Client)
                .Include(v => v.Utilisateur)
                .Include(v => v.LignesVente).ThenInclude(l => l.Produit)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task AddAsync(Vente vente)
        {
            await _context.Ventes.AddAsync(vente);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}