using SmartFlow.Application.DTOs.Auth;
using SmartFlow.Application.Interfaces;
using SmartFlow.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartFlow.Application.Services
{
    public class AuthService
    {
        private readonly IUtilisateurRepository _utilisateurRepository;
        private readonly PasswordService _passwordService;
        private readonly TokenService _tokenService;

        public AuthService(
            IUtilisateurRepository utilisateurRepository,
            PasswordService passwordService,
            TokenService tokenService)
        {
            _utilisateurRepository = utilisateurRepository;
            _passwordService = passwordService;
            _tokenService = tokenService;
        }

        public async Task<string> RegisterAsync(RegisterDto dto)
        {
            if (await _utilisateurRepository.EmailExisteAsync(dto.Email))
                throw new Exception("Cet email est déjà utilisé.");

            var utilisateur = new Utilisateur
            {
                NomComplet = dto.NomComplet,
                Email = dto.Email,
                MotDePasseHash = _passwordService.HashPassword(dto.MotDePasse),
                RoleId = 2    //Vendeur = rôle par défaut pour toute inscription publique
            };

            await _utilisateurRepository.AddAsync(utilisateur);

            return "Utilisateur créé avec succès.";
        }

        public async Task<string> LoginAsync(LoginDto dto)
        {
            var utilisateur = await _utilisateurRepository.GetByEmailAsync(dto.Email);

            if (utilisateur == null || !_passwordService.VerifyPassword(dto.MotDePasse, utilisateur.MotDePasseHash))
                throw new Exception("Email ou mot de passe incorrect.");

            return _tokenService.GenerateToken(utilisateur);
        }

        public async Task<string> CreateUtilisateurAsync(CreateUtilisateurDto dto)
        {
            if (await _utilisateurRepository.EmailExisteAsync(dto.Email))
                throw new Exception("Cet email est déjà utilisé.");

            var utilisateur = new Utilisateur
            {
                NomComplet = dto.NomComplet,
                Email = dto.Email,
                MotDePasseHash = _passwordService.HashPassword(dto.MotDePasse),
                RoleId = dto.RoleId
            };

            await _utilisateurRepository.AddAsync(utilisateur);

            return "Utilisateur créé avec succès.";
        }
    }
}
