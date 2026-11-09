using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.DTOs.Ventes
{
    public class LigneVenteCreateDto
    {
        public int ProduitId { get; set; }
        public int Quantite { get; set; }
    }
}
