using TrustZoneAPI.Models;

namespace TrustZoneAPI.Repositories.Interfaces
{
    public interface IAccessibilityFeatureRepository :IRepository<AccessibilityFeature,int>
    {
        Task<IEnumerable<AccessibilityFeature>> SearchByNameAsync(string keyword);
    }
}
