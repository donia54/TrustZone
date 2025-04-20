using TrustZoneAPI.Models;

namespace TrustZoneAPI.Services.Repositories.Interfaces
{
    public interface ISavedPlaceRepository :IRepository<SavedPlace,int>
    {
        Task<IEnumerable<SavedPlace>> GetSavedPlacesByUserAsync(string userId);
        Task<bool> IsBranchSavedByUserAsync(string userId, int branchId);
    }
}
