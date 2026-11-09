using SmartFlow.Application.DTOs.Dashboard;
using SmartFlow.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.Services
{
    public class DashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<DashboardStatsDto> GetStatsAsync()
        {
            // Requêtes exécutées l'une après l'autre (séquentiellement) :
            // un DbContext ne peut pas traiter plusieurs requêtes en même temps
            var chiffreAffaires = await _dashboardRepository.GetChiffreAffairesTotalAsync();
            var nombreVentes = await _dashboardRepository.GetNombreVentesAsync();
            var nombreClients = await _dashboardRepository.GetNombreClientsAsync();
            var nombreProduits = await _dashboardRepository.GetNombreProduitsAsync();
            var stockFaible = await _dashboardRepository.GetProduitsStockFaibleAsync();
            var ventesParJour = await _dashboardRepository.GetVentesParJourAsync(7);
            var topProduits = await _dashboardRepository.GetTopProduitsAsync(5);

            return new DashboardStatsDto
            {
                ChiffreAffairesTotal = chiffreAffaires,
                NombreVentes = nombreVentes,
                NombreClients = nombreClients,
                NombreProduits = nombreProduits,
                ProduitsStockFaible = stockFaible,
                VentesParJour = ventesParJour
                    .Select(v => new VenteParJourDto { Date = v.Date.ToString("yyyy-MM-dd"), Montant = v.Montant })
                    .ToList(),
                TopProduits = topProduits
                    .Select(p => new TopProduitDto { ProduitNom = p.ProduitNom, QuantiteVendue = p.QuantiteVendue })
                    .ToList()
            };
        }
    }
}
