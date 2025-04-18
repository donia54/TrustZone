using System.ComponentModel.DataAnnotations;
using TrustZoneAPI.DTOs.Users;

namespace TrustZoneAPI.DTOs.Chat;

public class ConversationDTO
{
    public int Id { get; set; }
    public string User1Id { get; set; } = null!;
    public string User2Id { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? LastMessageAt { get; set; }
    public UserBasicDTO? User1 { get; set; }
    public UserBasicDTO? User2 { get; set; }
}

public class CreateConversationDTO
{
    [Required(ErrorMessage = "User2Id is required.")]
    public string User2Id { get; set; } = null!;
}
