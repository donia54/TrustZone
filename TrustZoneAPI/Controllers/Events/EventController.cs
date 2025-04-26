using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.Services.Events;
using static TrustZoneAPI.DTOs.Events.EventsDTOs;

namespace TrustZoneAPI.Controllers.Events
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : BaseController
    {

        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var response = await _eventService.GetAllAsync();
            return MapResponseToActionResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _eventService.GetByIdAsync(id);
            return MapResponseToActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAccessibilityEventDto dto)
        {
            var response = await _eventService.CreateAsync(dto);
            return MapResponseToActionResult(response);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CreateAccessibilityEventDto dto)
        {
            var response = await _eventService.UpdateAsync(id, dto);
            return MapResponseToActionResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _eventService.DeleteAsync(id);
            return MapResponseToActionResult(response);
        }


      
        [HttpGet("branch/{branchId}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetByBranchId(int branchId)
        {
            var response = await _eventService.GetByBranchIdAsync(branchId);
            return MapResponseToActionResult(response);
        }

        [HttpGet("organizer/{organizerId}")]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<IActionResult> GetByOrganizerId(string organizerId)
        {
            var response = await _eventService.GetByOrganizerIdAsync(organizerId);
            return MapResponseToActionResult(response);
        }
    }
}
