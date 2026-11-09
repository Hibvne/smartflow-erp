using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Domain.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string Action { get; set; } // "Création client", "Suppression produit"...
        public DateTime DateAction { get; set; } = DateTime.Now;

        public int UtilisateurId { get; set; }
        public Utilisateur Utilisateur { get; set; }
    }
}
