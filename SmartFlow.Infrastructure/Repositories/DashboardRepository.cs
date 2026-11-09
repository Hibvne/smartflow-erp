using Microsoft.EntityFrameworkCore;
using SmartFlow.Application.Interfaces;
using SmartFlow.Infrastructure.Data;

namespace SmartFlow.Infrastructure.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly SmartFlowDbContext _context;

        public DashboardRepository(SmartFlowDbContext context)
        {
            _context = context;
        }

        public async Task<decimal> GetChiffreAffairesTotalAsync()
        {
            // Somme de toutes les ventes ; si aucune vente, renvoie 0 (pas d'exception)
            return await _context.Ventes.SumAsync(v => (decimal?)v.MontantTotal) ?? 0;
        }

        public async Task<int> GetNombreVentesAsync() =>
            await _context.Ventes.CountAsync();

        public async Task<int> GetNombreClientsAsync() =>
            await _context.Clients.CountAsync();

        public async Task<int> GetNombreProduitsAsync() =>
            await _context.Produits.CountAsync();

        public async Task<int> GetProduitsStockFaibleAsync() =>
            // On compare directement 2 colonnes de la même table, EF Core traduit ça en SQL WHERE
            await _context.Produits.CountAsync(p => p.QuantiteStock <= p.SeuilAlerte);

        public async Task<List<(DateTime Date, decimal Montant)>> GetVentesParJourAsync(int nbJours)
        {
            var dateDebut = DateTime.Now.Date.AddDays(-nbJours + 1);

            // GroupBy sur la date (sans l'heure) pour regrouper les ventes jour par jour
            var resultats = await _context.Ventes
                .Where(v => v.DateVente >= dateDebut)
                .GroupBy(v => v.DateVente.Date)
                .Select(g => new { Date = g.Key, Montant = g.Sum(v => v.MontantTotal) })
                .ToListAsync();

            return resultats.Select(r => (r.Date, r.Montant)).ToList();
        }

        public async Task<List<(string ProduitNom, int QuantiteVendue)>> GetTopProduitsAsync(int top)
        {
            var resultats = await _context.LignesVente
                .GroupBy(l => l.Produit.Nom)
                .Select(g => new { ProduitNom = g.Key, QuantiteVendue = g.Sum(l => l.Quantite) })
                .OrderByDescending(x => x.QuantiteVendue)
                .Take(top)
                .ToListAsync();

            return resultats.Select(r => (r.ProduitNom, r.QuantiteVendue)).ToList();
        }
    }
}