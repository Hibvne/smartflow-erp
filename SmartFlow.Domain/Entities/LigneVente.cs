using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Domain.Entities
{
    public class LigneVente
    {
        public int Id { get; set; }
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }

        public int VenteId { get; set; }
        public Vente Vente { get; set; }

        public int ProduitId { get; set; }
        public Produit Produit { get; set; }
    }
}
