using Microsoft.EntityFrameworkCore;
using TrustZoneAPI.Data;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Repositories
{
    public class EventRepository : IEventRepository
    {
        private readonly AppDbContext _context;

        public EventRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<AccessibilityEvent>> GetAllAsync()
        {
            return await _context.AccessibilityEvents
                .Include(e => e.Branch)
                .Include(e => e.Organizer)
                .ToListAsync();
        }

        public async Task<AccessibilityEvent?> GetByIdAsync(int id)
        {
            return await _context.AccessibilityEvents
                .Include(e => e.Branch)
                .Include(e => e.Organizer)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<bool> AddAsync(AccessibilityEvent entity)
        {
            await _context.AccessibilityEvents.AddAsync(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(AccessibilityEvent entity)
        {
            _context.AccessibilityEvents.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }


        public async Task<bool> DeleteAsync(int id)
        {
            var eventToDelete = await _context.AccessibilityEvents.FindAsync(id);
            if (eventToDelete == null)
                return false;

            _context.AccessibilityEvents.Remove(eventToDelete);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<AccessibilityEvent>> GetByBranchIdAsync(int branchId)
        {
            return await _context.AccessibilityEvents
                .Where(e => e.BranchId == branchId)
                .Include(e => e.Branch)
                .Include(e => e.Organizer)
                .ToListAsync();
        }

        public async Task<IEnumerable<AccessibilityEvent>> GetEventsByOrganizerIdAsync(string organizerId)
        {
            return await _context.AccessibilityEvents
                .Where(e => e.OrganizerId == organizerId)
                .Include(e => e.Branch)
                .Include(e => e.Organizer)
                .ToListAsync();
        }
    }
}
