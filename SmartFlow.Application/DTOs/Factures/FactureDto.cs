using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.DTOs.Factures
{
    public class FactureDto
    {
                public int Id { get; set; }
        public string NumeroFacture { get; set; }
        public DateTime DateFacture { get; set; }
        public decimal MontantTotal { get; set; }
        public string Statut { get; set; }

        public int VenteId { get; set; }
        public string ClientNom { get; set; }
    }
}
