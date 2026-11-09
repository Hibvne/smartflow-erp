namespace SmartFlow.Application.DTOs.Factures
{
    public class EnregistrerPaiementDto
    {
        public decimal Montant { get; set; }
        public string ModePaiement { get; set; } // "Espèces", "Virement", "Carte"
    }
}