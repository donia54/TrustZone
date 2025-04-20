using TrustZoneAPI.Repositories;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Services.Disabilities
{
    public interface IDisabilityTypeService
    {
        Task<bool> IsDisabilityTypeExistsAsync(int id);
    }
    public class DisabilityTypeService : IDisabilityTypeService
    {

        private readonly IDisabilityTypeRepository _disabilityTypeRepository;

        public DisabilityTypeService(IDisabilityTypeRepository disabilityTypeRepository)
        {
            _disabilityTypeRepository = disabilityTypeRepository;
        }

        public async Task<bool> IsDisabilityTypeExistsAsync(int id)
        {
            return await _disabilityTypeRepository.IsDisabilityTypeExistsAsync(id);
        }
    }
}
