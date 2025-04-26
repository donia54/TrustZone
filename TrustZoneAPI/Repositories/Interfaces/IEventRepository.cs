using TrustZoneAPI.Models;

namespace TrustZoneAPI.Repositories.Interfaces
{
    public interface IEventRepository :IRepository<AccessibilityEvent, int>
    {
        Task<IEnumerable<AccessibilityEvent>> GetByBranchIdAsync(int branchId);
        Task<IEnumerable<AccessibilityEvent>> GetEventsByOrganizerIdAsync(string organizerId);
    }
}
