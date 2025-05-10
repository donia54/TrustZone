using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.DTOs.Chat;
using TrustZoneAPI.Services.Chat;

namespace TrustZoneAPI.Controllers.chat
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ConversationController : BaseController
    {
        private readonly IConversationService _conversationService;

        public ConversationController(IConversationService conversationService)
        {
            _conversationService = conversationService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ConversationDTO>> GetById(int id)
        {
            var result = await _conversationService.GetByIdAsync(id);
            return MapResponseToActionResult(result);
        }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<ConversationDTO>>> GetConversationsByUserId( [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var result = await _conversationService.GetConversationsByUserIdAsync(CurrentUserId, page, pageSize);
            return MapResponseToActionResult(result);
        }

        [HttpGet("between")]
        public async Task<ActionResult<ConversationDTO>> GetConversationBetweenUsers( [FromQuery] string user2Id)
        {
            var result = await _conversationService.GetConversationBetweenUsersAsync(CurrentUserId, user2Id);
            return MapResponseToActionResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<int>> Create([FromBody] CreateConversationDTO createDto)
        {
            var result = await _conversationService.CreateAsync(createDto.User2Id);
            return MapResponseToActionResult(result);
        }

        [HttpPut("{id}/last-message")]
        public async Task<ActionResult> UpdateLastMessageAt(int id, [FromBody] UpdateConversationDTO updateDto)
        {
            var success = await _conversationService.UpdateLastMessageAtAsync(id, updateDto.LastMessageAt ?? DateTime.UtcNow);
            return success ? Ok() : NotFound("Conversation not found.");
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _conversationService.DeleteAsync(id);
            return MapResponseToActionResult(result);
        }
    }
}
