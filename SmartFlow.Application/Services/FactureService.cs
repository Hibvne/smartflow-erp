using SmartFlow.Application.DTOs.Clients;
using SmartFlow.Application.DTOs.Factures;
using SmartFlow.Application.Interfaces;
using SmartFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.Services
{
    public class FactureService
    {
        private readonly IFactureRepository _factureRepository;
        private readonly IPaiementRepository _paiementRepository;

        public FactureService(IFactureRepository factureRepository, IPaiementRepository paiementRepository)
        {
            _factureRepository = factureRepository;
            _paiementRepository = paiementRepository;
        }

        private FactureDto ToDto(Facture f) => new FactureDto
        {
            Id = f.Id,
            NumeroFacture = f.NumeroFacture,
            DateFacture = f.DateFacture,
            MontantTotal = f.MontantTotal,
            Statut = f.Statut,
            VenteId = f.VenteId,
            ClientNom = f.Vente?.Client?.Nom
        };

        public async Task<PagedResultDto<FactureDto>> GetPagedAsync(int pageNumber, int pageSize)
        {
            var (items, totalCount) = await _factureRepository.GetPagedAsync(pageNumber, pageSize);
            return new PagedResultDto<FactureDto>
            {
                Items = items.Select(ToDto).ToList(),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<FactureDto> GetByIdAsync(int id)
        {
            var facture = await _factureRepository.GetByIdAsync(id);
            if (facture == null) throw new Exception("Facture introuvable.");
            return ToDto(facture);
        }

        public async Task<string> EnregistrerPaiementAsync(int factureId, EnregistrerPaiementDto dto)
        {
            var facture = await _factureRepository.GetByIdAsync(factureId);
            if (facture == null) throw new Exception("Facture introuvable.");

            if (facture.Statut == "Payée")
                throw new Exception("Cette facture est déjà payée.");

            var paiement = new Paiement
            {
                Montant = dto.Montant,
                ModePaiement = dto.ModePaiement,
                FactureId = facture.Id
            };

            await _paiementRepository.AddAsync(paiement);

            facture.Statut = "Payée"; // on simplifie : un seul paiement suffit à couvrir la facture (V1)

            await _factureRepository.SaveChangesAsync();
            return "Paiement enregistré, facture marquée comme payée.";
        }
    }
}
