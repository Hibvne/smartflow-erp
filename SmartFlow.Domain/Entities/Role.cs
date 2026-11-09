using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Domain.Entities
{
    public class Role
    {
        public int Id { get; set; }
        public string Nom { get; set; } // Admin, Vendeur, Magasinier...
        public ICollection<Utilisateur> Utilisateurs { get; set; }
    }
}
