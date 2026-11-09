using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.DTOs.Auth
{
    public class CreateUtilisateurDto
    {
        public string NomComplet { get; set; }
        public string Email { get; set; }
        public string MotDePasse { get; set; }
        public int RoleId { get; set; }
    }
}
