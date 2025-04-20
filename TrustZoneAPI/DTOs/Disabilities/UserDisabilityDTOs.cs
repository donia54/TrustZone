using System.ComponentModel.DataAnnotations;

namespace TrustZoneAPI.DTOs.Disabilities
{
    public class UserDisabilityCreateDTO
    {
        [Required(ErrorMessage = "DisabilityTypeId is required")]
        public int DisabilityTypeId { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; } = null!;
    }

}
