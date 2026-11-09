using System.ComponentModel.DataAnnotations;

namespace SmartFlow.Application.DTOs.Auth
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Le nom complet est obligatoire.")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères.")]
        public string NomComplet { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire.")]
        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [MinLength(8, ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères.")]
        public string MotDePasse { get; set; }
    }
}