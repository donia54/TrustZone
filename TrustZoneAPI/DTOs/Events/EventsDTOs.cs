using TrustZoneAPI.DTOs.Places;

namespace TrustZoneAPI.DTOs.Events
{
    public class EventsDTOs
    {
        public class AccessibilityEventDto
        {
            public int Id { get; set; }
            public string EventName { get; set; } = null!;
            public string? Description { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string? SpecialFeaturesAvailable { get; set; }
            public DateTime CreatedAt { get; set; }

            public BranchLightDTO Branch { get; set; } = null!;


        }

        public class CreateAccessibilityEventDto
        {
            public int BranchId { get; set; }
            public string EventName { get; set; } = null!;
            public string? Description { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public string? SpecialFeaturesAvailable { get; set; }
           // public string? OrganizerId { get; set; }
        }
    }
}
