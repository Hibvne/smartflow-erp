using SmartFlow.Application.Interfaces;
using SmartFlow.Domain.Entities;
using SmartFlow.Infrastructure.Data;

namespace SmartFlow.Infrastructure.Repositories
{
    public class AuditLogRepository : IAuditLogRepository
    {
        private readonly SmartFlowDbContext _context;

        public AuditLogRepository(SmartFlowDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(AuditLog log)
        {
            // Pas de SaveChanges séparé ici : on le sauvegarde en même temps que
            // l'opération qui déclenche le log (voir ProduitService), pour rester atomique
            await _context.AuditLogs.AddAsync(log);
        }
    }
}