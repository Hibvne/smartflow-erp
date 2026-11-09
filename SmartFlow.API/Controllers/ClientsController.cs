using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFlow.Application.DTOs.Clients;
using SmartFlow.Application.Services;

namespace SmartFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Toutes les routes de ce contrôleur exigent un token JWT valide
    public class ClientsController : ControllerBase
    {
        private readonly ClientService _clientService;

        public ClientsController(ClientService clientService)
        {
            _clientService = clientService;
        }

        // GET /api/Clients?pageNumber=1&pageSize=10&search=xxx
        // Liste paginée + recherche, utilisée pour l'affichage du tableau côté frontend
        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = null)
        {
            var result = await _clientService.GetPagedAsync(pageNumber, pageSize, search);
            return Ok(result);
        }

        // GET /api/Clients/5
        // Récupère un seul client par son Id, utilisé par exemple pour la page "détail" ou "édition"
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var client = await _clientService.GetByIdAsync(id);
                return Ok(client);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message); // Client introuvable -> 404, pas 500
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClientDto dto)
        {
            var result = await _clientService.CreateAsync(dto);
            return Ok(result);
        }

        // PUT /api/Clients/5
        // Remplace les infos du client existant par celles du DTO envoyé
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateClientDto dto)
        {
            try
            {
                var result = await _clientService.UpdateAsync(id, dto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        // DELETE /api/Clients/5
        // Restreint aux Admins uniquement : suppression définitive, action sensible
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _clientService.DeleteAsync(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
