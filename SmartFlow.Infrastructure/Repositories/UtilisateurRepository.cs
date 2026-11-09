using Microsoft.EntityFrameworkCore;
using SmartFlow.Application.Interfaces;
using SmartFlow.Domain.Entities;
using SmartFlow.Infrastructure.Data;

namespace SmartFlow.Infrastructure.Repositories
{
    public class UtilisateurRepository : IUtilisateurRepository
    {
        private readonly SmartFlowDbContext _context;

        public UtilisateurRepository(SmartFlowDbContext context)
        {
            _context = context;
        }

        public async Task<Utilisateur> GetByEmailAsync(string email)
        {
            return await _context.Utilisateurs
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExisteAsync(string email)
        {
            return await _context.Utilisateurs.AnyAsync(u => u.Email == email);
        }

        public async Task AddAsync(Utilisateur utilisateur)
        {
            _context.Utilisateurs.Add(utilisateur);
            await _context.SaveChangesAsync();
        }
    }
}