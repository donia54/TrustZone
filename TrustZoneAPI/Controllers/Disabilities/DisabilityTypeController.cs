using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.DTOs.Disabilities;
using TrustZoneAPI.Services.Disabilities;

namespace TrustZoneAPI.Controllers.Disabilities
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class DisabilityTypeController : BaseController
    {

        private readonly IDisabilityTypeService _disabilityTypeService;

        public DisabilityTypeController(IDisabilityTypeService disabilityTypeService)
        {
            _disabilityTypeService = disabilityTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _disabilityTypeService.GetAllAsync();
            return MapResponseToActionResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _disabilityTypeService.GetByIdAsync(id);
            return MapResponseToActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CreateDisabilityTypeDto dto)
        {
            var response = await _disabilityTypeService.AddAsync(dto);
            return MapResponseToActionResult(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateDisabilityTypeDto dto)
        {
            var response = await _disabilityTypeService.UpdateAsync(id, dto);
            return MapResponseToActionResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _disabilityTypeService.DeleteAsync(id);
            return MapResponseToActionResult(response);
        }
    }
}
