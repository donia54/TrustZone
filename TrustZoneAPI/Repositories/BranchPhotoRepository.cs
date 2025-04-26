using Microsoft.EntityFrameworkCore;
using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Repositories
{
    public class BranchPhotoRepository: IBranchPhotoRepository
    {

        private readonly AppDbContext _context;

        public BranchPhotoRepository(AppDbContext context)
        {
            _context = context;
        }


        public async Task<IEnumerable<BranchPhoto>> GetAllAsync()
        {
            return await _context.BranchPhotos
                                 .Include(p => p.Branch)
                                 .Include(p => p.UploadedByUser)
                                 .ToListAsync();
        }

        public async Task<BranchPhoto?> GetByIdAsync(int id)
        {
            return await _context.BranchPhotos
                                 .Include(p => p.Branch)
                                 .Include(p => p.UploadedByUser)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> AddAsync(BranchPhoto entity)
        {
            _context.BranchPhotos.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(BranchPhoto entity)
        {
            _context.BranchPhotos.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.BranchPhotos.FindAsync(id);
            if (entity == null) return false;

            _context.BranchPhotos.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<BranchPhoto>> GetPhotosByBranchIdAsync(int branchId)
        {
            return await _context.BranchPhotos
                                 .Where(p => p.BranchId == branchId)
                                 .ToListAsync();
        }

        public async Task<IEnumerable<BranchPhoto>> GetApprovedPhotosAsync(int branchId)
        {
            return await _context.BranchPhotos
                                 .Where(p => p.BranchId == branchId && p.IsApproved)
                                 .ToListAsync();
        }

        public async Task<BranchPhoto?> GetFeaturedPhotoAsync(int branchId)
        {
            return await _context.BranchPhotos
                                 .FirstOrDefaultAsync(p => p.BranchId == branchId && p.IsFeatured);
        }



        public async Task<BranchPhoto?> GetMainPhotoByBranchIdAsync(int branchId, int? preferredPhotoId = null)
        {
            var photos = await GetPhotosByBranchIdAsync(branchId);

            if (photos == null || !photos.Any())
                return null;

            if (preferredPhotoId.HasValue)
            {
                var preferredPhoto = photos.FirstOrDefault(p => p.Id == preferredPhotoId.Value);
                if (preferredPhoto != null)
                    return preferredPhoto;
            }

            return photos.OrderBy(p => p.UploadDate).FirstOrDefault();
        }


    }
}
