using Microsoft.EntityFrameworkCore;
using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
using TrustZoneAPI.Services.Repositories.Interfaces;

namespace TrustZoneAPI.Services.Repositories
{
    public class BranchRepository : IBranchRepository
    {
        private readonly AppDbContext _context;

        public BranchRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Branch>> GetAllAsync()
        {
            return await _context.Branches.ToListAsync();
        }

        public async Task<Branch?> GetByIdAsync(int id)
        {
            return await _context.Branches.FindAsync(id);
        }

        public async Task<bool> AddAsync(Branch entity)
        {
            await _context.Branches.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Branch entity)
        {
            _context.Branches.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var branch = await _context.Branches.FindAsync(id);
            if (branch == null)
                return false;

            _context.Branches.Remove(branch);
            return await _context.SaveChangesAsync() > 0;
        }


        #region We used GetPlaceWithBranchesByIdAsync in place repo instead.
        /*public async Task<IEnumerable<Branch>> GetBranchesByPlaceIdAsync(int placeId)
        {
            return await _context.Branches.Where(b => b.PlaceId == placeId).ToListAsync();
        }*/
        #endregion

    }
}
