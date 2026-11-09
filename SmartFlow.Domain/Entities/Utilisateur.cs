using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Domain.Entities
{
    public class Utilisateur
    {
        public int Id { get; set; }
        public string NomComplet { get; set; }
        public string Email { get; set; }
        public string MotDePasseHash { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }

        public DateTime DateCreation { get; set; } = DateTime.Now;
    }
}
