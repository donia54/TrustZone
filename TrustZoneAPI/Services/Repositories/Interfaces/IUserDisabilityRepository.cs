using TrustZoneAPI.Models;

namespace TrustZoneAPI.Services.Repositories.Interfaces
{
    public interface IUserDisabilityRepository :IRepository<UserDisability,int>
    {
        Task<List<DisabilityType>> GetUserDisabilitiesByUserIdAsync(string userId);

        Task SetUserDisabilityTypesAsync(string userId, List<int> disabilityTypeIds);
    }
}
