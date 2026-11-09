using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.DTOs.Ventes
{
    public class LigneVenteDto
    {
        public int ProduitId { get; set; }
        public string ProduitNom { get; set; }
        public int Quantite { get; set; }
        public decimal PrixUnitaire { get; set; }
        public decimal SousTotal => Quantite * PrixUnitaire; // calculé, pas stocké
    }
}
