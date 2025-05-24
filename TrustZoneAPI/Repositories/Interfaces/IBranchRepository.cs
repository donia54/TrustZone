using TrustZoneAPI.Models;

namespace TrustZoneAPI.Repositories.Interfaces
{
    public interface IBranchRepository :IRepository<Branch,int>
    {
        Task<IEnumerable<Branch>> GetBranchesByCategoryIdAsync(int categoryId);
        Task<IEnumerable<Branch>> FilterBranchesByPlaceFeaturesAsync(List<int> featureIds);
        Task<IEnumerable<Branch>> GetBranchesWithFeatureAsync(int featureId);
        Task<IEnumerable<Branch>> SearchBranchesAsync(string query ,int page, int pageSize);



    }
}
