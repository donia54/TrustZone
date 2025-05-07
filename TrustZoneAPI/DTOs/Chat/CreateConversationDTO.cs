using System.ComponentModel.DataAnnotations;

namespace TrustZoneAPI.DTOs.Chat;

public class CreateConversationDTO
{
    [Required(ErrorMessage = "User2Id is required.")]
    public string User2Id { get; set; } = null!;
}
