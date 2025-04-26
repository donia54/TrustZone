using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.DTOs.Places;
using TrustZoneAPI.Services.Places;

namespace TrustZoneAPI.Controllers.Places
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : BaseController
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchDTO>>> GetAll()
        {
            var result = await _branchService.GetAllAsync();
            return MapResponseToActionResult(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BranchDTO>> GetById(int id)
        {
            var result = await _branchService.GetByIdAsync(id);
            return MapResponseToActionResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<BranchDTO>> Create([FromBody] CreateBranchDTO createDto)
        {
            var result = await _branchService.AddAsync(createDto);
            return MapResponseToActionResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateBranchDTO updateDto)
        {
            var result = await _branchService.UpdateAsync(id, updateDto);
            return MapResponseToActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _branchService.DeleteAsync(id);
            return MapResponseToActionResult(result);
        }



        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var result = await _branchService.GetBranchesByCategoryIdAsync(categoryId);
            return MapResponseToActionResult(result);
        }


        [HttpGet("feature/{featureId}")]
        public async Task<IActionResult> GetByFeature(int featureId)
        {
            var result = await _branchService.GetBranchesWithFeatureAsync(featureId);
            return MapResponseToActionResult(result);
        }

        [HttpPost("filter")]
        public async Task<ActionResult> FilterPlacesByFeaturesAsync([FromBody] List<int> featureIds)
        {
            var result = await _branchService.FilterBranchesByFeaturesAsync(featureIds);
            return MapResponseToActionResult(result);
        }


        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Search query cannot be empty.");

            var result = await _branchService.SearchBranchesAsync(query, page, pageSize);
            return Ok(result);
        }
    }
}
