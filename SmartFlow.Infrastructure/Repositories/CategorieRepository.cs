using Microsoft.EntityFrameworkCore;
using SmartFlow.Application.Interfaces;
using SmartFlow.Domain.Entities;
using SmartFlow.Infrastructure.Data;

namespace SmartFlow.Infrastructure.Repositories
{
    public class CategorieRepository : ICategorieRepository
    {
        private readonly SmartFlowDbContext _context;

        public CategorieRepository(SmartFlowDbContext context)
        {
            _context = context;
        }

        public async Task<List<Categorie>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Categorie> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task AddAsync(Categorie categorie)
        {
            await _context.Categories.AddAsync(categorie);
        }

        public void Update(Categorie categorie)
        {
            _context.Categories.Update(categorie);
        }

        public void Delete(Categorie categorie)
        {
            _context.Categories.Remove(categorie);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}