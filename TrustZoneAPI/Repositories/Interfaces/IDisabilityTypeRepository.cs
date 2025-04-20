using TrustZoneAPI.Models;

namespace TrustZoneAPI.Repositories.Interfaces
{
    public interface IDisabilityTypeRepository :IRepository<DisabilityType,int>
    {
        Task<bool> IsDisabilityTypeExistsAsync(int disabilityTypeId);


    }
}
