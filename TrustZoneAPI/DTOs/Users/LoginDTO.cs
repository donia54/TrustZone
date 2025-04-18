using System.ComponentModel.DataAnnotations;

namespace TrustZoneAPI.DTOs.Users
{
    public class LoginDTO
    {
        public string? Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
        public bool RememberMe { get; set; }
    }

}
