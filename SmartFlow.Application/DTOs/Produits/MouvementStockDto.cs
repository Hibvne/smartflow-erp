using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.DTOs.Produits
{
    public class MouvementStockDto
    {
        public int Quantite { get; set; } // toujours positif, le sens (+/-) dépend de la route appelée
        public string Motif { get; set; } // ex: "Réapprovisionnement", "Casse", "Inventaire"
    }
}
