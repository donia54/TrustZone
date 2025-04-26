namespace TrustZoneAPI.DTOs.AccessibilityFeatures
{
    public class AddFeatureToPlaceDTO
    {
        public int PlaceId { get; set; }
        public int FeatureId { get; set; }
    }

    public class AddFeatureListToPlaceDTO
    {
        public int PlaceId { get; set; }
        public List<int> FeatureIds { get; set; } = new();
    }
}
