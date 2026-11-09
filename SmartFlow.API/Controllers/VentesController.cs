using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFlow.Application.DTOs.Ventes;
using SmartFlow.Application.Services;
using System.Security.Claims;

namespace SmartFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class VentesController : ControllerBase
    {
        private readonly VenteService _venteService;

        public VentesController(VenteService venteService)
        {
            _venteService = venteService;
        }

        private int GetUtilisateurId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await _venteService.GetPagedAsync(pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try { return Ok(await _venteService.GetByIdAsync(id)); }
            catch (Exception ex) { return NotFound(ex.Message); }
        }

        // Crée la vente + décrémente le stock + génère la facture, en une seule action
        [HttpPost]
        public async Task<IActionResult> Create(CreateVenteDto dto)
        {
            try { return Ok(await _venteService.CreateAsync(dto, GetUtilisateurId())); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
