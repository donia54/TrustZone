using TrustZoneAPI.Models;

namespace TrustZoneAPI.Services.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User,string>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByDisabilityAsync(int disabilityTypeId);
        Task<bool> IsUserExists(string userId);
    }
}
