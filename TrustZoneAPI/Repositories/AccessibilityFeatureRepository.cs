using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;
using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Repositories
{
    public class AccessibilityFeatureRepository : IAccessibilityFeatureRepository
    {
        private readonly AppDbContext _context;
        public AccessibilityFeatureRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<AccessibilityFeature>> GetAllAsync()
        {
            return await _context.AccessibilityFeatures.ToListAsync();
        }

        public async Task<AccessibilityFeature?> GetByIdAsync(int id)
        {
            return await _context.AccessibilityFeatures.FindAsync(id);
        }

        public async Task<bool> AddAsync(AccessibilityFeature entity)
        {
            await _context.AccessibilityFeatures.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(AccessibilityFeature entity)
        {
            _context.AccessibilityFeatures.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var feature = await _context.AccessibilityFeatures.FindAsync(id);
            if (feature == null) return false;

            _context.AccessibilityFeatures.Remove(feature);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<AccessibilityFeature>> SearchByNameAsync(string keyword)
        {
            return await _context.AccessibilityFeatures
                .Where(f => EF.Functions.Like(f.FeatureName.ToLower(), $"%{keyword.ToLower()}%")
                         || (f.Description != null && EF.Functions.Like(f.Description.ToLower(), $"%{keyword.ToLower()}%")))
                .ToListAsync();
        }


    }
}
