using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Repositories
{
    public class DisabilityTypeRepository : IDisabilityTypeRepository
    {
        public Task<bool> AddAsync(DisabilityType entity)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<DisabilityType>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<DisabilityType?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

    

        public Task<bool> UpdateAsync(DisabilityType entity)
        {
            throw new NotImplementedException();
        }
    }
}
