using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.DTOs.Disabilities;
using TrustZoneAPI.Services.Disabilities;

namespace TrustZoneAPI.Controllers.Disabilities
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserDisabilitiesController : BaseController
    {

        private readonly IUserDisabilityService _userDisabilityService;

        public UserDisabilitiesController(IUserDisabilityService userDisabilityService)
        {
            _userDisabilityService = userDisabilityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _userDisabilityService.GetAllAsync();
            return MapResponseToActionResult(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _userDisabilityService.GetByIdAsync(id);
            return MapResponseToActionResult(result);
        }

  

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userDisabilityService.DeleteAsync(id);
            return MapResponseToActionResult(result);
        }

        [HttpGet("user-disabilities")]
        public async Task<IActionResult> GetUserDisabilitiesByUserId()
        {
            var result = await _userDisabilityService.GetUserDisabilitiesByUserIdAsync(CurrentUserId);
            return MapResponseToActionResult(result);
        }

        [HttpPut("user/{userId}")]
        public async Task<IActionResult> SetUserDisabilityTypes( [FromBody] List<int> disabilityTypeIds)
        {
            var result = await _userDisabilityService.SetUserDisabilityTypesAsync(CurrentUserId, disabilityTypeIds);
            return MapResponseToActionResult(result);
        }
    }
}
