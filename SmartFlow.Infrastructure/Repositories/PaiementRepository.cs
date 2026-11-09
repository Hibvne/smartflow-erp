using SmartFlow.Application.Interfaces;
using SmartFlow.Domain.Entities;
using SmartFlow.Infrastructure.Data;

namespace SmartFlow.Infrastructure.Repositories
{
    public class PaiementRepository : IPaiementRepository
    {
        private readonly SmartFlowDbContext _context;

        public PaiementRepository(SmartFlowDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Paiement paiement)
        {
            await _context.Paiements.AddAsync(paiement);
            // Pas de SaveChanges ici : sauvegardé avec la mise à jour du statut de la Facture, comme un seul bloc
        }
    }
}