using TrustZoneAPI.Models;

namespace TrustZoneAPI.Services.Repositories.Interfaces
{
    public interface IBranchRepository :IRepository<Branch,int>
    {
        //Task<IEnumerable<Branch>> GetBranchesByPlaceIdAsync(int placeId);
       // Task<IEnumerable<Branch>> SearchBranchesAsync(string searchTerm);

    }
}
