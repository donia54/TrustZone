using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.DTOs.Chat;
using TrustZoneAPI.Services.Chat;

namespace TrustZoneAPI.Controllers.chat
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class MessageController : BaseController
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("conversation/{conversationId}")]
        public async Task<ActionResult<IEnumerable<MessageDTO>>> GetMessagesByConversation(int conversationId, [FromQuery] int page = 1)
        {
            var result = await _messageService.GetMessagesByConversationAsync(conversationId, page);
            return MapResponseToActionResult(result);
        }

        [HttpPost]
        public async Task<ActionResult> Create([FromBody] CreateMessageDTO createDto)
        {
            var result = await _messageService.CreateAsync(createDto,CurrentUserId);
            return MapResponseToActionResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateMessageDTO updateDto)
        {
            var result = await _messageService.UpdateAsync(id, updateDto);
            return MapResponseToActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _messageService.DeleteAsync(id, CurrentUserId);
            return MapResponseToActionResult(result);
        }

        [HttpPut("{id}/mark-as-read")]
        public async Task<ActionResult> MarkAsRead(int id)
        {
            var result = await _messageService.MarkAsReadAsync(id);
            return MapResponseToActionResult(result);
        }
    }
}
