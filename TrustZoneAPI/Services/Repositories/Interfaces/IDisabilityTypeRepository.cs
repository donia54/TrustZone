using TrustZoneAPI.Models;

namespace TrustZoneAPI.Services.Repositories.Interfaces
{
    public interface IDisabilityTypeRepository :IRepository<DisabilityType,int>
    {
        Task<bool> IsDisabilityTypeExistsAsync(int disabilityTypeId);


    }
}
