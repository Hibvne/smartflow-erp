using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.DTOs.Ventes
{
    public class VenteDto
    {
        public int Id { get; set; }
        public DateTime DateVente { get; set; }
        public decimal MontantTotal { get; set; }

        public int ClientId { get; set; }
        public string ClientNom { get; set; }

        public string VendeurNom { get; set; }

        public List<LigneVenteDto> Lignes { get; set; }

        public string NumeroFacture { get; set; } // pratique d'avoir le lien direct vers sa facture
    }
}

