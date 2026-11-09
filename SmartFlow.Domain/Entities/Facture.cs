using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Domain.Entities
{
    public class Facture
    {
        public int Id { get; set; }
        public string NumeroFacture { get; set; }
        public DateTime DateFacture { get; set; } = DateTime.Now;
        public decimal MontantTotal { get; set; }
        public string Statut { get; set; } // "Payée", "En attente", "Annulée"

        public int VenteId { get; set; }
        public Vente Vente { get; set; }
    }
}
