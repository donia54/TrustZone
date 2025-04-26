using System.ComponentModel.DataAnnotations;

namespace TrustZoneAPI.DTOs.Disabilities
{

    public class UserDisabilityDto
    {
        public int Id { get; set; }
        public int DisabilityTypeId { get; set; }
        public string UserId { get; set; } = null!;
        public string DisabilityTypeName { get; set; } = null!;
    }
    public class UserDisabilityCreateDTO
    {
        [Required(ErrorMessage = "DisabilityTypeId is required")]
        public int DisabilityTypeId { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; } = null!;
    }

    public class SetUserDisabilitiesDto
    {
        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; } = null!;
        public List<int> DisabilityTypeIds { get; set; } = new();
    }

}
