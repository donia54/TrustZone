using TrustZoneAPI.Models;

namespace TrustZoneAPI.Repositories.Interfaces
{
    public interface IReviewRepository:IRepository<Review,int>
    {
        Task<IEnumerable<Review>> GetReviewsByBranchAsync(int branchId, int pageNumber, int pageSize);
        Task<IEnumerable<Review>> GetReviewsByUserAsync(string userId);
        Task<IEnumerable<Review>> GetVerifiedReviewsAsync();
        Task<Review> VerifyReviewAsync(int id);

      //  Task<bool> UpdateContentUrlAsync(int reviewId, string contentUrl);


    }
}
