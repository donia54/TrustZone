using TrustZoneAPI.Models;

namespace TrustZoneAPI.Services.Repositories.Interfaces
{
    public interface IPlaceRepository : IRepository<Place,int>
    {
        Task<IEnumerable<Place>> GetPlacesByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Place>> GetPlacesWithFeatureAsync(int featureId);
        Task<IEnumerable<Place>> SearchPlacesAsync(string query, int page, int pageSize);
        Task<IEnumerable<Place>> FilterPlacesByFeaturesAsync(List<int> featureIds);

        Task<Place?> GetPlaceWithBranchesByIdAsync(int placeId);
    }
}
