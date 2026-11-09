using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.DTOs.Dashboard
{
    public class DashboardStatsDto
    {
        public decimal ChiffreAffairesTotal { get; set; }
        public int NombreVentes { get; set; }
        public int NombreClients { get; set; }
        public int NombreProduits { get; set; }
        public int ProduitsStockFaible { get; set; }

        // Pour le graphique Chart.js : ventes des 7 derniers jours
        public List<VenteParJourDto> VentesParJour { get; set; }

        // Top 5 produits les plus vendus
        public List<TopProduitDto> TopProduits { get; set; }
    }

    public class VenteParJourDto
    {
        public string Date { get; set; } // format "2026-08-10"
        public decimal Montant { get; set; }
    }

    public class TopProduitDto
    {
        public string ProduitNom { get; set; }
        public int QuantiteVendue { get; set; }
    }
}
