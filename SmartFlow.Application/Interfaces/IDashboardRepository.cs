using SmartFlow.Domain.Entities;

namespace SmartFlow.Application.Interfaces
{
    public interface IDashboardRepository
    {
        Task<decimal> GetChiffreAffairesTotalAsync();
        Task<int> GetNombreVentesAsync();
        Task<int> GetNombreClientsAsync();
        Task<int> GetNombreProduitsAsync();
        Task<int> GetProduitsStockFaibleAsync();
        Task<List<(DateTime Date, decimal Montant)>> GetVentesParJourAsync(int nbJours);
        Task<List<(string ProduitNom, int QuantiteVendue)>> GetTopProduitsAsync(int top);
    }
}