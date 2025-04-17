using TrustZoneAPI.Models;

namespace TrustZoneAPI.Repositories.Interfaces
{
    public interface IUserRepository : IRepository<User,string>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<IEnumerable<User>> GetUsersByDisabilityAsync(int disabilityTypeId);
    }
}
