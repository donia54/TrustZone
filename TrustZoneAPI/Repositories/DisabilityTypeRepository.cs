using Microsoft.EntityFrameworkCore;
using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Repositories
{
    public class DisabilityTypeRepository : IDisabilityTypeRepository
    {
        private readonly AppDbContext _context;

        public DisabilityTypeRepository(AppDbContext context)
        {
            _context = context;
        }
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



        public async Task<bool> IsDisabilityTypeExistsAsync(int disabilityTypeId)
        {
            return await _context.DisabilityTypes.AnyAsync(d => d.Id == disabilityTypeId);
        }
    }
}
