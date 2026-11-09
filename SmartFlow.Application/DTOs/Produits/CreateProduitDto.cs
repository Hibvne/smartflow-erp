using System.ComponentModel.DataAnnotations;

namespace SmartFlow.Application.DTOs.Produits
{
    public class CreateProduitDto
    {
        [Required(ErrorMessage = "Le nom est obligatoire.")]
        [StringLength(150, ErrorMessage = "Le nom ne peut pas dépasser 150 caractères.")]
        public string Nom { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Le prix d'achat ne peut pas être négatif.")]
        public decimal PrixAchat { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Le prix de vente ne peut pas être négatif.")]
        public decimal PrixVente { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Le stock ne peut pas être négatif.")]
        public int QuantiteStock { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "Le seuil d'alerte ne peut pas être négatif.")]
        public int SeuilAlerte { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "La catégorie est obligatoire.")]
        public int CategorieId { get; set; }
    }
}