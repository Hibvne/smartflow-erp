using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Domain.Entities
{
    public class Categorie
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Description { get; set; }
        public string Prefixe { get; set; } // ex: "VET", "FRU", "HYG"
    }
}
