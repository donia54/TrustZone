using TrustZoneAPI.Models;

namespace TrustZoneAPI.Services.Repositories.Interfaces
{
    public interface IBranchOpeningHourRepository : IRepository<BranchOpeningHour,int>
    {
        Task<IEnumerable<BranchOpeningHour>> GetByBranchIdAsync(int branchId);
    }
}
