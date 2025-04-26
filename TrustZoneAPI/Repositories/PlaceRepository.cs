using Microsoft.EntityFrameworkCore;
using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Repositories
{
    public class PlaceRepository : IPlaceRepository
    {
        private readonly AppDbContext _context;

        public PlaceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Place>> GetAllAsync()
        {
            return await _context.Places
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.PlaceFeatures)
                    .ThenInclude(pf => pf.Feature)
                .ToListAsync();
        }

        public async Task<Place?> GetByIdAsync(int id)
        {
            return await _context.Places
                .AsNoTracking()
                .Include(p => p.Category)
                .Include(p => p.PlaceFeatures)
                    .ThenInclude(pf => pf.Feature)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> AddAsync(Place place)
        {
            await _context.Places.AddAsync(place);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Place place)
        {
            _context.Places.Update(place);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var place = await _context.Places.FindAsync(id);
            if (place == null)
                return false;

            _context.Places.Remove(place);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Place>> GetPlacesByCategoryIdAsync(int categoryId)
        {
            return await _context.Places
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.PlaceFeatures)
                    .ThenInclude(pf => pf.Feature)
                .ToListAsync();
        }

     
        public async Task<IEnumerable<Place>> FilterPlacesByFeaturesAsync(List<int> featureIds)
        {
            return await _context.Places
                .AsNoTracking()
                .Where(p => featureIds.All(fid => p.PlaceFeatures.Any(pf => pf.FeatureId == fid)))
                .ToListAsync();
        }


        public async Task<IEnumerable<Place>> GetPlacesWithFeatureAsync(int featureId)
        {
            return await _context.Places
                .AsNoTracking()
                .Where(p => p.PlaceFeatures.Any(pf => pf.FeatureId == featureId))

                .ToListAsync();
        }


        public async Task<IEnumerable<Place>> SearchPlacesAsync(string query, int page, int pageSize)
        {
            return await _context.Places
           .Where(p => p.Name.Contains(query) || p.Category.Name.Contains(query))
           .OrderBy(p => p.Name)
           .Skip((page - 1) * pageSize)
           .Take(pageSize)
           .ToListAsync();
        }

  
        public async Task<Place?> GetPlaceWithBranchesByIdAsync(int placeId)
        {
            return await _context.Places
                .AsNoTracking()
                .Include(p => p.Branches)
                .FirstOrDefaultAsync(p => p.Id == placeId);
        }
    }
}
