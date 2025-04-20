using Microsoft.EntityFrameworkCore;
using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Repositories
{
    public class PlaceFeatureRepository : IPlaceFeatureRepository
    {
        private readonly AppDbContext _context;

        public PlaceFeatureRepository(AppDbContext context)
        {
            _context = context;
        }

        public async  Task<IEnumerable<PlaceFeature>> GetFeaturesByPlaceIdAsync(int placeId)
        {
            return await _context.PlaceFeatures
            .Where(pf => pf.PlaceId == placeId)
            .Include(pf => pf.Feature)
            .ToListAsync();
        }
        public async Task<bool> AddFeatureToPlaceAsync(PlaceFeature placeFeature)
        {
            _context.PlaceFeatures.Add(placeFeature);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> RemoveFeatureFromPlaceAsync(PlaceFeature placeFeature)
        {
            _context.PlaceFeatures.Remove(placeFeature);
            await _context.SaveChangesAsync();
            return true;

        }

        public async Task<bool> IsFeatureExistsAsync(int placeId, int featureId)
        {
            return await _context.PlaceFeatures
            .AnyAsync(pf => pf.PlaceId == placeId && pf.FeatureId == featureId);

        }
    }
}
