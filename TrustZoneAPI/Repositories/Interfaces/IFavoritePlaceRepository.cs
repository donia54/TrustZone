using TrustZoneAPI.Models;

namespace TrustZoneAPI.Repositories.Interfaces
{
    public interface IFavoritePlaceRepository :IRepository<FavoritePlace,int>
    {
        Task<IEnumerable<FavoritePlace>> GetFavoritePlacesByUserAsync(string userId);
        Task<bool> IsBranchFavoritedByUserAsync(string userId, int branchId);
    }
}
