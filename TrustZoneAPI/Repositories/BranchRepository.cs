using Microsoft.EntityFrameworkCore;
using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Repositories
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

        public async Task<IEnumerable<Branch>> GetBranchesByCategoryIdAsync(int categoryId)
        {
            return await  _context.Branches
                      .AsNoTracking()
                      .Where(b => b.Place.CategoryId == categoryId)
                     .ToListAsync();
        }


        public async Task<IEnumerable<Branch>> FilterBranchesByPlaceFeaturesAsync(List<int> featureIds)
        {
            return await _context.Branches
                .AsNoTracking()
                .Where(b => featureIds.All(fid => b.Place.PlaceFeatures.Any(pf => pf.FeatureId == fid)))
                .ToListAsync();
        }

        public async Task<IEnumerable<Branch>> GetBranchesWithFeatureAsync(int featureId)
        {
            return await _context.Branches
                .AsNoTracking()
                .Where(b => b.Place.PlaceFeatures.Any(pf => pf.FeatureId == featureId))
                .ToListAsync();
        }

        public async Task<IEnumerable<Branch>> SearchBranchesAsync(string query, int page, int pageSize)
        {
            return await _context.Branches
                .AsNoTracking()
                 .Include(b => b.Place)
                .Where(b => b.Place.Name.Contains(query) || b.Place.Category.Name.Contains(query) || b.Place.Details.Contains(query))
                .OrderBy(b => b.Place.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }



    }
}
