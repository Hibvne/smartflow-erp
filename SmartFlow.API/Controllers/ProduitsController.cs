using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFlow.Application.DTOs.Produits;
using SmartFlow.Application.Services;
using System.Security.Claims;

namespace SmartFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Toutes les routes de ce contrôleur exigent un token JWT valide
    public class ProduitsController : ControllerBase
    {
        private readonly ProduitService _produitService;

        public ProduitsController(ProduitService produitService)
        {
            _produitService = produitService;
        }

        // Récupère l'Id de l'utilisateur connecté depuis le token JWT (claim posé lors du login)
        private int GetUtilisateurId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

        // GET /api/Produits?pageNumber=1&pageSize=10&search=xxx
        [HttpGet]
        public async Task<IActionResult> GetPaged(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string search = null)
        {
            return Ok(await _produitService.GetPagedAsync(pageNumber, pageSize, search));
        }

        // GET /api/Produits/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try { return Ok(await _produitService.GetByIdAsync(id)); }
            catch (Exception ex) { return NotFound(ex.Message); }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateProduitDto dto)
        {
            return Ok(await _produitService.CreateAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateProduitDto dto)
        {
            try { return Ok(await _produitService.UpdateAsync(id, dto)); }
            catch (Exception ex) { return NotFound(ex.Message); }
        }

        // Suppression restreinte aux Admins : action sensible
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try { return Ok(await _produitService.DeleteAsync(id)); }
            catch (Exception ex) { return NotFound(ex.Message); }
        }

        // POST /api/Produits/5/entree-stock
        // Ajoute de la quantité au stock (ex: réapprovisionnement) + trace l'action dans AuditLog
        [HttpPost("{id}/entree-stock")]
        public async Task<IActionResult> EntreeStock(int id, MouvementStockDto dto)
        {
            try { return Ok(await _produitService.EntreeStockAsync(id, dto, GetUtilisateurId())); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }

        // POST /api/Produits/5/sortie-stock
        // Retire de la quantité au stock (ex: vente, casse) + trace l'action + bloque si stock insuffisant
        [HttpPost("{id}/sortie-stock")]
        public async Task<IActionResult> SortieStock(int id, MouvementStockDto dto)
        {
            try { return Ok(await _produitService.SortieStockAsync(id, dto, GetUtilisateurId())); }
            catch (Exception ex) { return BadRequest(ex.Message); }
        }
    }
}
