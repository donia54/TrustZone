using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.DTOs.AccessibilityFeatures;
using TrustZoneAPI.Services.AccessibilityFeatures;

namespace TrustZoneAPI.Controllers.AccessibilityFeatures
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccessibilityFeaturesController : BaseController
    {

        private readonly IAccessibilityFeatureService _accessibilityFeatureService;

        public AccessibilityFeaturesController(IAccessibilityFeatureService accessibilityFeatureService)
        {
            _accessibilityFeatureService = accessibilityFeatureService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var response = await _accessibilityFeatureService.GetAllAsync();
            return MapResponseToActionResult(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var response = await _accessibilityFeatureService.GetByIdAsync(id);
            return MapResponseToActionResult(response);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] CreateAccessibilityFeatureDTO dto)
        {
            var response = await _accessibilityFeatureService.AddAsync(dto);
            return MapResponseToActionResult(response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] CreateAccessibilityFeatureDTO dto)
        {
            var response = await _accessibilityFeatureService.UpdateAsync(id, dto);
            return MapResponseToActionResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _accessibilityFeatureService.DeleteAsync(id);
            return MapResponseToActionResult(response);
        }

        [HttpGet("search")]
        public async Task<ActionResult> Search([FromQuery] string keyword)
        {
            var response = await _accessibilityFeatureService.SearchAsync(keyword);
            return MapResponseToActionResult(response);
        }
    }
}
