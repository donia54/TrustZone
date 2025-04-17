using TrustZoneAPI.Models;

namespace TrustZoneAPI.Repositories.Interfaces
{
    public interface IUserDisabilityRepository :IRepository<UserDisability,int>
    {
        Task<IEnumerable<UserDisability>> GetUserDisabilitiesAsync(string userId);
    }
}
