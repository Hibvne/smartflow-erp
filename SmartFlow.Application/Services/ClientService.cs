using SmartFlow.Application.DTOs;
using SmartFlow.Application.DTOs.Clients;
using SmartFlow.Application.Interfaces;
using SmartFlow.Domain.Entities;

namespace SmartFlow.Application.Services
{
    public class ClientService
    {
        private readonly IClientRepository _clientRepository;

        public ClientService(IClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        public async Task<PagedResultDto<ClientDto>> GetPagedAsync(int pageNumber, int pageSize, string search)
        {
            var (items, totalCount) = await _clientRepository.GetPagedAsync(pageNumber, pageSize, search);

            var dtos = items.Select(c => new ClientDto
            {
                Id = c.Id,
                Nom = c.Nom,
                Email = c.Email,
                Telephone = c.Telephone,
                Adresse = c.Adresse,
                DateCreation = c.DateCreation
            }).ToList();

            return new PagedResultDto<ClientDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<ClientDto> GetByIdAsync(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null) throw new Exception("Client introuvable.");

            return new ClientDto
            {
                Id = client.Id,
                Nom = client.Nom,
                Email = client.Email,
                Telephone = client.Telephone,
                Adresse = client.Adresse,
                DateCreation = client.DateCreation
            };
        }

        public async Task<string> CreateAsync(CreateClientDto dto)
        {
            var client = new Client
            {
                Nom = dto.Nom,
                Email = dto.Email,
                Telephone = dto.Telephone,
                Adresse = dto.Adresse
            };

            await _clientRepository.AddAsync(client);
            await _clientRepository.SaveChangesAsync();

            return "Client créé avec succès.";
        }

        public async Task<string> UpdateAsync(int id, UpdateClientDto dto)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null) throw new Exception("Client introuvable.");

            client.Nom = dto.Nom;
            client.Email = dto.Email;
            client.Telephone = dto.Telephone;
            client.Adresse = dto.Adresse;

            _clientRepository.Update(client);
            await _clientRepository.SaveChangesAsync();

            return "Client mis à jour avec succès.";
        }

        public async Task<string> DeleteAsync(int id)
        {
            var client = await _clientRepository.GetByIdAsync(id);
            if (client == null) throw new Exception("Client introuvable.");

            _clientRepository.Delete(client);
            await _clientRepository.SaveChangesAsync();

            return "Client supprimé avec succès.";
        }
    }
}