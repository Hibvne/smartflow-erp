using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFlow.Application.Services;
using SmartFlow.Application.DTOs.Factures;

namespace SmartFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class FacturesController : ControllerBase
    {
        private readonly FactureService _factureService;

        public FacturesController(FactureService factureService)
        {
            _factureService = factureService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            return Ok(await _factureService.GetPagedAsync(pageNumber, pageSize));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try { return Ok(await _factureService.GetByIdAsync(id)); }
            catch (Exception ex) { return NotFound(ex.Message); }
        }

        [HttpPost("{id}/paiement")]
        public async Task<IActionResult> EnregistrerPaiement(int id, EnregistrerPaiementDto dto)
        {
            try { return Ok(await _factureService.EnregistrerPaiementAsync(id, dto)); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
