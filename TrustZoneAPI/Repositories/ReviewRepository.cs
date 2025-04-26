using Microsoft.EntityFrameworkCore;
using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Repositories
{
    public class ReviewRepository :IReviewRepository
    {
        private readonly AppDbContext _context;

        public ReviewRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Review>> GetAllAsync()
        {
            return await _context.Reviews.ToListAsync();
        }

        public async Task<Review?> GetByIdAsync(int id)
        {
            return await _context.Reviews.FindAsync(id);
        }

        public async Task<bool> AddAsync(Review entity)
        {
            await _context.Reviews.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Review entity)
        {
            _context.Reviews.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
                return false;

            _context.Reviews.Remove(review);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Review>> GetReviewsByBranchAsync(int branchId, int pageNumber, int pageSize)
        {
            return await _context.Reviews
                .Where(r => r.BranchId == branchId)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Include(r => r.User)
                .ToListAsync();
        }


        public async Task<IEnumerable<Review>> GetReviewsByUserAsync(string userId)
        {
            return await _context.Reviews
                .Where(r => r.UserId == userId)
                .Include(r => r.Branch)
                .Include(r=>r.User)
                .ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetVerifiedReviewsAsync()
        {
            return await _context.Reviews
                .Where(r => r.IsVerified == true)
                .Include(r => r.User)
                .Include(r => r.Branch)
                .ToListAsync();
        }

        public async Task<Review> VerifyReviewAsync(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            review.IsVerified = true;
            await _context.SaveChangesAsync();
            return review;
        }
    }
}
