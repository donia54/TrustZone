using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.DTOs.Places;
using TrustZoneAPI.Services;
using TrustZoneAPI.Services.Places;

namespace TrustZoneAPI.Controllers.Places
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceController : BaseController
    {
        private readonly IPlaceService _PlaceService;

        public PlaceController (IPlaceService placeService)
        {
            _PlaceService = placeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _PlaceService.GetAllAsync();
            return MapResponseToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _PlaceService.GetByIdAsync(id);
            return MapResponseToActionResult(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreatePlaceDTO placeDto)
        {
            if (placeDto == null)
                return BadRequest("Invalid place data.");

            var result = await _PlaceService.AddAsync(placeDto);
            return MapResponseToActionResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreatePlaceDTO placeDto)
        {
            if (placeDto == null)
                return BadRequest("Invalid place data.");

            var result = await _PlaceService.UpdateAsync(id, placeDto);
            return MapResponseToActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _PlaceService.DeleteAsync(id);
            return MapResponseToActionResult(result);
        }

        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(int categoryId)
        {
            var result = await _PlaceService.GetPlacesByCategoryIdAsync(categoryId);
            return MapResponseToActionResult(result);
        }


        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("feature/{featureId}")]
        public async Task<IActionResult> GetByFeature(int featureId)
        {
            var result = await _PlaceService.GetPlacesWithFeatureAsync(featureId);
            return MapResponseToActionResult(result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPost("filter")]
        public async Task<ActionResult> FilterPlacesByFeaturesAsync([FromBody] List<int> featureIds)
        {
            var result = await _PlaceService.FilterPlacesByFeaturesAsync(featureIds);
            return MapResponseToActionResult(result);
        }


        [HttpGet("with_branches{placeId}")]
        public async Task<ActionResult> GetPlaceWithBranchesByIdAsync(int placeId)
        {
            var result = await _PlaceService.GetPlaceWithBranchesByIdAsync(placeId);
            return MapResponseToActionResult(result);
        }



        [HttpGet("search")]
        public async Task<IActionResult> Search([FromQuery] string query, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            if (string.IsNullOrWhiteSpace(query))
                return BadRequest("Search query cannot be empty.");

            var result = await _PlaceService.SearchPlacesAsync(query, page, pageSize);
            return Ok(result);
        }
    }
}
