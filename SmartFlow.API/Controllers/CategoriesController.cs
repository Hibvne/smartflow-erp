using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartFlow.Application.DTOs.Categories;
using SmartFlow.Application.Services;

namespace SmartFlow.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
        private readonly CategorieService _categorieService;

        public CategoriesController(CategorieService categorieService)
        {
            _categorieService = categorieService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categorieService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try { return Ok(await _categorieService.GetByIdAsync(id)); }
            catch (Exception ex) { return NotFound(ex.Message); }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCategorieDto dto)
        {
            return Ok(await _categorieService.CreateAsync(dto));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCategorieDto dto)
        {
            try { return Ok(await _categorieService.UpdateAsync(id, dto)); }
            catch (Exception ex) { return NotFound(ex.Message); }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try { return Ok(await _categorieService.DeleteAsync(id)); }
            catch (Exception ex) { return NotFound(ex.Message); }
        }
    }
}
