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

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserDisability>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserDisability?> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<DisabilityType>> GetUserDisabilitiesByUserIdAsync(string userId)
        {
            return await _context.UserDisabilities
           .Where(ud => ud.UserId == userId)
           .Select(ud => ud.DisabilityType)
           .ToListAsync();
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

        public Task<bool> UpdateAsync(UserDisability entity)
        {
            throw new NotImplementedException();
        }
    }
}
