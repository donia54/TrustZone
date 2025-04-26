using Microsoft.EntityFrameworkCore;
using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Repositories
{
    public class FavoritePlaceRepository:IFavoritePlaceRepository
    {
        private readonly AppDbContext _context;

        public FavoritePlaceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FavoritePlace>> GetAllAsync()
        {
            return await _context.FavoritePlaces.ToListAsync();
        }

        public async Task<FavoritePlace?> GetByIdAsync(int id)
        {
            return await _context.FavoritePlaces.FindAsync(id);
        }

        public async Task<bool> AddAsync(FavoritePlace entity)
        {
            await _context.FavoritePlaces.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(FavoritePlace entity)
        {
            _context.FavoritePlaces.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteByBranchIdAndUserIdAsync(int branchId, string userId)
        {
            var favoritePlace = await _context.FavoritePlaces
                .FirstOrDefaultAsync(fp => fp.BranchId == branchId && fp.UserId == userId);

            if (favoritePlace == null)
                return false;

            _context.FavoritePlaces.Remove(favoritePlace);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var favoritePlace = await _context.FavoritePlaces.FindAsync(id);
            if (favoritePlace == null)
                return false;

            _context.FavoritePlaces.Remove(favoritePlace);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<IEnumerable<FavoritePlace>> GetFavoritePlacesByUserAsync(string userId)
        {
            return await _context.FavoritePlaces
                .Where(fp => fp.UserId == userId)
                .Include(fp => fp.Branch)
                .ToListAsync();
        }

        public async Task<bool> IsBranchFavoritedByUserAsync(string userId, int branchId)
        {
            return await _context.FavoritePlaces
                .AnyAsync(fp => fp.UserId == userId && fp.BranchId == branchId);
        }

    }
}
