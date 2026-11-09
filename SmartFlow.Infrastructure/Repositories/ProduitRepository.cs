using Microsoft.EntityFrameworkCore;
using SmartFlow.Application.Interfaces;
using SmartFlow.Domain.Entities;
using SmartFlow.Infrastructure.Data;

namespace SmartFlow.Infrastructure.Repositories
{
    public class ProduitRepository : IProduitRepository
    {
        private readonly SmartFlowDbContext _context;

        public ProduitRepository(SmartFlowDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Produit> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string search)
        {
            // .Include() charge la Categorie liée en même temps (sinon CategorieNom serait vide dans le DTO)
            var query = _context.Produits.Include(p => p.Categorie).AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(p =>
                    p.Nom.Contains(search) ||
                    p.Reference.Contains(search));
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(p => p.Nom)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<Produit> GetByIdAsync(int id)
        {
            // Include ici aussi, sinon produit.Categorie serait null
            return await _context.Produits.Include(p => p.Categorie).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task AddAsync(Produit produit)
        {
            await _context.Produits.AddAsync(produit);
        }

        public void Update(Produit produit)
        {
            _context.Produits.Update(produit);
        }

        public void Delete(Produit produit)
        {
            _context.Produits.Remove(produit);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetCountAsync()
        {
            return await _context.Produits.CountAsync();
        }

        public async Task<int> GetCountByCategorieAsync(int categorieId)
        {
            return await _context.Produits.CountAsync(p => p.CategorieId == categorieId);
        }
    }
}