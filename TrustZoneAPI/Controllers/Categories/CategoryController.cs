using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.DTOs.Categories;
using TrustZoneAPI.Services.Categories;

namespace TrustZoneAPI.Controllers.Categories
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {

        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _categoryService.GetAllAsync();
            return MapResponseToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _categoryService.GetByIdAsync(id);
            return MapResponseToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDTO dto)
        {
            var result = await _categoryService.CreateAsync(dto);
            return MapResponseToActionResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateCategoryDTO dto)
        {
            var result = await _categoryService.UpdateAsync(id, dto);
            return MapResponseToActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _categoryService.DeleteAsync(id);
            return MapResponseToActionResult(result);
        }
    }
}
