using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Domain.Entities
{
    public class Produit
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Reference { get; set; }
        public decimal PrixAchat { get; set; }
        public decimal PrixVente { get; set; }
        public int QuantiteStock { get; set; }
        public int SeuilAlerte { get; set; }

        public int CategorieId { get; set; }
        public Categorie Categorie { get; set; }
    }
}
