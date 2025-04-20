using Microsoft.EntityFrameworkCore;
using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
using TrustZoneAPI.Services.Repositories.Interfaces;

namespace TrustZoneAPI.Services.Repositories
{
    public class BranchOpeningHourRepository : IBranchOpeningHourRepository
    {
        private readonly AppDbContext _context;

        public BranchOpeningHourRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BranchOpeningHour>> GetAllAsync()
        {
            return await _context.BranchOpeningHours.ToListAsync();
        }

        public async Task<BranchOpeningHour?> GetByIdAsync(int id)
        {
            return await _context.BranchOpeningHours.FindAsync(id);
        }

        public async Task<bool> AddAsync(BranchOpeningHour branchOpeningHour)
        {
            _context.BranchOpeningHours.Add(branchOpeningHour);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(BranchOpeningHour branchOpeningHour)
        {
            _context.BranchOpeningHours.Update(branchOpeningHour);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.BranchOpeningHours.FindAsync(id);
            if (entity != null)
            {
                _context.BranchOpeningHours.Remove(entity);
                return await _context.SaveChangesAsync() > 0;
            }
            return false;
        }


        public async Task<IEnumerable<BranchOpeningHour>> GetByBranchIdAsync(int branchId)
        {
            return await _context.BranchOpeningHours
                .Where(x => x.BranchId == branchId)
                .ToListAsync();
        }


    }
}
