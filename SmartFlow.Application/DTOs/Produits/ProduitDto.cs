using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.DTOs.Produits
{
    public class ProduitDto
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Reference { get; set; }
        public decimal PrixAchat { get; set; }
        public decimal PrixVente { get; set; }
        public int QuantiteStock { get; set; }
        public int SeuilAlerte { get; set; }

        public int CategorieId { get; set; }
        public string CategorieNom { get; set; } // pratique pour l'affichage direct côté frontend

        public bool StockFaible => QuantiteStock <= SeuilAlerte; // calculé automatiquement, pas stocké en base
    }
}
