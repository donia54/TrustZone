using Microsoft.EntityFrameworkCore;
using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Repositories
{
    public class UserDisabilityRepository : IUserDisabilityRepository
    {

        private readonly AppDbContext _context;

        public UserDisabilityRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddAsync(UserDisability entity)
        {
            _context.UserDisabilities.Add(entity);
           return  await _context.SaveChangesAsync() > 0;
        }

        public async  Task<bool> DeleteAsync(int id)
        {
            var entity = await  _context.UserDisabilities.FindAsync(id);
            if (entity == null)
                return false;

            _context.UserDisabilities.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<UserDisability>> GetAllAsync()
        {
            return await _context.UserDisabilities
                .Include(ud => ud.DisabilityType)
                .ToListAsync();
        }

        public async Task<UserDisability?> GetByIdAsync(int id)
        {
            return await _context.UserDisabilities
                .Include(ud => ud.DisabilityType)
                .FirstOrDefaultAsync(ud => ud.Id == id);
        }

        public async Task<List<DisabilityType>> GetUserDisabilitiesByUserIdAsync(string userId)
        {
            return await _context.UserDisabilities
           .Where(ud => ud.UserId == userId)
           .Select(ud => ud.DisabilityType)
           .ToListAsync();
        }


        public async Task<UserDisability?> GetByUserIdAsync(string userId)
        {
            return await _context.UserDisabilities
                .FirstOrDefaultAsync(ud => ud.UserId == userId);
        }

        public async Task SetUserDisabilityTypesAsync(string userId, List<int> disabilityTypeIds)
        {
            var newDisabilities = disabilityTypeIds.Select(id => new UserDisability
            {
                UserId = userId,
                DisabilityTypeId = id
            });

            await _context.UserDisabilities.AddRangeAsync(newDisabilities);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(UserDisability entity)
        {
            var existingEntity = await _context.UserDisabilities.FindAsync(entity.Id);
            if (existingEntity == null)
                return false;

            existingEntity.UserId = entity.UserId;
            existingEntity.DisabilityTypeId = entity.DisabilityTypeId;

            _context.UserDisabilities.Update(existingEntity);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}
