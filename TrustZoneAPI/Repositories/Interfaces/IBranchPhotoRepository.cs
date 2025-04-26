using TrustZoneAPI.Models;

namespace TrustZoneAPI.Repositories.Interfaces
{
    public interface IBranchPhotoRepository : IRepository<BranchPhoto, int>
    {
        Task<IEnumerable<BranchPhoto>> GetPhotosByBranchIdAsync(int branchId);
        Task<IEnumerable<BranchPhoto>> GetApprovedPhotosAsync(int branchId);
        Task<BranchPhoto?> GetFeaturedPhotoAsync(int branchId);

        Task<BranchPhoto?> GetMainPhotoByBranchIdAsync(int branchId, int? preferredPhotoId = null);

    }
}
