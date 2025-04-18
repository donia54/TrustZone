using System.ComponentModel.DataAnnotations;
using TrustZoneAPI.DTOs.Users;
using TrustZoneAPI.Services.Messages;

namespace TrustZoneAPI.DTOs.Chat
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public int ConversationId { get; set; }
        public string SenderId { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime SentAt { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public UserBasicDTO? Sender { get; set; }
    }
    public class CreateMessageDTO
    {
        [Required(ErrorMessage = "Conversation ID is required")]
        public int ConversationId { get; set; }

        [Required(ErrorMessage = "Message content is required")]
        [StringLength(2000, ErrorMessage = "Message content must be between 1 and 2000 characters", MinimumLength = 1)]
        public string Content { get; set; } = null!;
    }

}
