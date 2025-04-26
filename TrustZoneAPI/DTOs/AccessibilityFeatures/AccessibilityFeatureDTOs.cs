namespace TrustZoneAPI.DTOs.AccessibilityFeatures
{
    public class CreateAccessibilityFeatureDTO
    {
        public string FeatureName { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class AccessibilityFeatureDTO
    {
        public int FeatureId { get; set; }
        public string FeatureName { get; set; } = null!;
        public string? Description { get; set; }
    }
}
