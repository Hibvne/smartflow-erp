using System.ComponentModel.DataAnnotations;

namespace SmartFlow.Application.DTOs.Clients
{
    public class CreateClientDto
    {
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(100, ErrorMessage = "Le nom ne peut pas dépasser 100 caractères.")]
        public string Nom { get; set; }

        [Required(ErrorMessage = "L'email est obligatoire.")]
        [EmailAddress(ErrorMessage = "Format d'email invalide.")]
        public string Email { get; set; }

        [Phone(ErrorMessage = "Format de téléphone invalide.")]
        public string Telephone { get; set; }

        [StringLength(250, ErrorMessage = "L'adresse ne peut pas dépasser 250 caractères.")]
        public string Adresse { get; set; }
    }
}