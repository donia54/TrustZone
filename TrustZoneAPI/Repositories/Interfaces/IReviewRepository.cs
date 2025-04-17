using TrustZoneAPI.Models;

namespace TrustZoneAPI.Repositories.Interfaces
{
    public interface IReviewRepository:IRepository<Review,int>
    {
        Task<IEnumerable<Review>> GetReviewsByBranchAsync(int branchId);
        Task<IEnumerable<Review>> GetReviewsByUserAsync(string userId);
        Task<IEnumerable<Review>> GetVerifiedReviewsAsync();
        Task<Review> VerifyReviewAsync(int id);

    }
}
