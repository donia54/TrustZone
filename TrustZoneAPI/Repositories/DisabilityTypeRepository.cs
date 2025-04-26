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
        public async Task<bool> AddAsync(DisabilityType entity)
        {
            await _context.DisabilityTypes.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var disabilityType = await _context.DisabilityTypes.FindAsync(id);
            if (disabilityType == null)
                return false;

            _context.DisabilityTypes.Remove(disabilityType);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<DisabilityType>> GetAllAsync()
        {
            return await _context.DisabilityTypes.ToListAsync();
        }
        public async Task<DisabilityType?> GetByIdAsync(int id)
        {
            return await _context.DisabilityTypes.FindAsync(id);
        }

        public async Task<bool> UpdateAsync(DisabilityType entity)
        {
            var existing = await _context.DisabilityTypes.FindAsync(entity.Id);
            if (existing == null)
                return false;

            existing.Name = entity.Name;


            _context.DisabilityTypes.Update(existing);
            return await _context.SaveChangesAsync() > 0;
        }



        public async Task<bool> IsDisabilityTypeExistsAsync(int disabilityTypeId)
        {
            return await _context.DisabilityTypes.AnyAsync(d => d.Id == disabilityTypeId);
        }
    }
}
