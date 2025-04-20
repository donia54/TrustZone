using TrustZoneAPI.Models;

namespace TrustZoneAPI.Repositories.Interfaces
{
    public interface IPlaceFeatureRepository
    {
        Task<IEnumerable<PlaceFeature>> GetFeaturesByPlaceIdAsync(int placeId);
        Task<bool> AddFeatureToPlaceAsync(PlaceFeature placeFeature);
        Task<bool> RemoveFeatureFromPlaceAsync(PlaceFeature placeFeature);
        Task<bool> IsFeatureExistsAsync(int placeId, int featureId);
    }
}
