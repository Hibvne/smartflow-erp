using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Domain.Entities
{
    public class Vente
    {
        public int Id { get; set; }
        public DateTime DateVente { get; set; } = DateTime.Now;
        public decimal MontantTotal { get; set; }

        public int ClientId { get; set; }
        public Client Client { get; set; }

        public int UtilisateurId { get; set; }
        public Utilisateur Utilisateur { get; set; }

        public ICollection<LigneVente> LignesVente { get; set; }
    }
}
