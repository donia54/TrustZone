using Microsoft.EntityFrameworkCore;
using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
using TrustZoneAPI.Services.Repositories.Interfaces;

namespace TrustZoneAPI.Services.Repositories
{
    public class CategoryRepository :ICategoryRepository
    {
        private readonly AppDbContext _context;

        public CategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await _context.Categories.FindAsync(id);
        }

        public async Task<bool> AddAsync(Category entity)
        {
            await _context.Categories.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Category entity)
        {
            _context.Categories.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
                return false;

            _context.Categories.Remove(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> IsCategoryExistsByNameAsync(string name)
        {
            return await _context.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<bool> IsCategoryExistsByIdAsync(int id)
        {
            return await _context.Categories.AnyAsync(c => c.Id == id);
        }


    }
}
