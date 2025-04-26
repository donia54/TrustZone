using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.DTOs.AccessibilityFeatures;
using TrustZoneAPI.Services.Places;

namespace TrustZoneAPI.Controllers.Places
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlaceFeatureController : BaseController
    {

        private readonly IPlaceFeatureService _placeFeatureService;

        public PlaceFeatureController(IPlaceFeatureService placeFeatureService)
        {
            _placeFeatureService = placeFeatureService;
        }

        [HttpPost("add")]
        public async Task<IActionResult> AddFeatureToPlace([FromBody] AddFeatureToPlaceDTO dto)
        {
            var result = await _placeFeatureService.AddFeatureToPlaceAsync(dto);
            return MapResponseToActionResult(result);
        }

        [HttpDelete("remove")]
        public async Task<IActionResult> RemoveFeatureFromPlace([FromBody] AddFeatureToPlaceDTO dto)
        {
            var result = await _placeFeatureService.RemoveFeatureFromPlaceAsync(dto);
            return MapResponseToActionResult(result);
        }

        [HttpPost("add-list")]
        public async Task<IActionResult> AddFeatureListToPlace([FromBody] AddFeatureListToPlaceDTO dto)
        {
            var result = await _placeFeatureService.AddFeatureListToPlaceAsync(dto);
            return MapResponseToActionResult(result);
        }

        [HttpGet("{placeId}/features")]
        public async Task<IActionResult> GetFeaturesByPlaceId(int placeId)
        {
            var result = await _placeFeatureService.GetFeaturesByPlaceIdAsync(placeId);
            return MapResponseToActionResult(result);
        }
    }
}
