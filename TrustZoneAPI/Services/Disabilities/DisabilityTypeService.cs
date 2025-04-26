using NuGet.Protocol.Core.Types;
using TrustZoneAPI.DTOs.Disabilities;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Services.Disabilities
{
    public interface IDisabilityTypeService
    {

        Task<ResponseResult<IEnumerable<DisabilityTypeDTO>>> GetAllAsync();
        Task<ResponseResult<DisabilityTypeDTO>> GetByIdAsync(int id);
        Task<ResponseResult<bool>> AddAsync(CreateDisabilityTypeDto dto);
        Task<ResponseResult> UpdateAsync(int id,CreateDisabilityTypeDto dto);
        Task<ResponseResult> DeleteAsync(int id);
        Task<bool> IsDisabilityTypeExistsAsync(int id);
    }
    public class DisabilityTypeService : IDisabilityTypeService
    {

        private readonly IDisabilityTypeRepository _disabilityTypeRepository;

        public DisabilityTypeService(IDisabilityTypeRepository disabilityTypeRepository)
        {
            _disabilityTypeRepository = disabilityTypeRepository;
        }

        public async Task<ResponseResult<IEnumerable<DisabilityTypeDTO>>> GetAllAsync()
        {
            try
            {
                var disabilityTypes = await _disabilityTypeRepository.GetAllAsync();
                var result = disabilityTypes.Select(d => new DisabilityTypeDTO
                {
                    Id = d.Id,
                    Name = d.Name
                });
                return ResponseResult<IEnumerable<DisabilityTypeDTO>>.Success(result);
            }
            catch (Exception ex)
            {
                return ResponseResult<IEnumerable<DisabilityTypeDTO>>.FromException(ex);
            }
        }

        public async Task<ResponseResult<DisabilityTypeDTO>> GetByIdAsync(int id)
        {
            try
            {
                var disabilityType = await _disabilityTypeRepository.GetByIdAsync(id);
                if (disabilityType == null)
                    return ResponseResult<DisabilityTypeDTO>.NotFound("Disability Type not found");

                return new DisabilityTypeDTO
                {
                    Id = disabilityType.Id,
                    Name = disabilityType.Name
                };
            }
            catch (Exception ex)
            {
                return ResponseResult<DisabilityTypeDTO>.FromException(ex);
            }
        }

        public async Task<ResponseResult<bool>> AddAsync(CreateDisabilityTypeDto dto)
        {
            try
            {
                var entity = new DisabilityType { Name = dto.Name };
                var result = await _disabilityTypeRepository.AddAsync(entity);
                if (!result)
                    return ResponseResult<bool>.Error("Failed to add Disability Type", 400);

                return ResponseResult<bool>.Created();
            }
            catch (Exception ex)
            {
                return ResponseResult<bool>.FromException(ex);
            }
        }

        public async Task<ResponseResult> UpdateAsync(int id,CreateDisabilityTypeDto dto)
        {
            try
            {
                var exists = await _disabilityTypeRepository.IsDisabilityTypeExistsAsync(id);
                if (!exists)
                    return ResponseResult.NotFound("Disability Type not found");

                var entity = new DisabilityType { Id = id, Name = dto.Name };

                var result = await _disabilityTypeRepository.UpdateAsync(entity);
                if (!result)
                    return ResponseResult.Error("Failed to update Disability Type", 400);

                return ResponseResult.Success();
            }
            catch (Exception ex)
            {
                return ResponseResult.FromException(ex);
            }
        }

        public async Task<ResponseResult> DeleteAsync(int id)
        {
            try
            {
                var exists = await _disabilityTypeRepository.IsDisabilityTypeExistsAsync(id);
                if (!exists)
                    return ResponseResult.NotFound("Disability Type not found");

                var result = await _disabilityTypeRepository.DeleteAsync(id);
                if (!result)
                    return ResponseResult.Error("Failed to delete Disability Type", 400);

                return ResponseResult.Success();
            }
            catch (Exception ex)
            {
                return ResponseResult.FromException(ex);
            }
        }


        public async Task<bool> IsDisabilityTypeExistsAsync(int id)
        {
            return await _disabilityTypeRepository.IsDisabilityTypeExistsAsync(id);
        }
    }
}
