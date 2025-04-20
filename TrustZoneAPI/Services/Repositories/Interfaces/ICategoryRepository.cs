using TrustZoneAPI.Models;

namespace TrustZoneAPI.Services.Repositories.Interfaces
{
    public interface ICategoryRepository : IRepository<Category,int>
    {
        Task<bool> IsCategoryExistsByNameAsync(string name);

        Task<bool> IsCategoryExistsByIdAsync(int id);

    }
}
