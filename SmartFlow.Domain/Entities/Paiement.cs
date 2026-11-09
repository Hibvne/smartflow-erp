using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Domain.Entities
{
    public class Paiement
    {
        public int Id { get; set; }
        public decimal Montant { get; set; }
        public DateTime DatePaiement { get; set; } = DateTime.Now;
        public string ModePaiement { get; set; } // "Espèces", "Virement", "Carte"

        public int FactureId { get; set; }
        public Facture Facture { get; set; }
    }
}
