using Microsoft.EntityFrameworkCore;
using TrustZoneAPI.Data;
using TrustZoneAPI.DTOs.Places;
using TrustZoneAPI.Models;
using TrustZoneAPI.Services.Repositories.Interfaces;

namespace TrustZoneAPI.Services.Places
{
    public interface IBranchOpeningHourService
    {
        Task<ResponseResult<IEnumerable<BranchOpeningHourDTO>>> GetAllAsync();
        Task<ResponseResult<BranchOpeningHourDTO>> GetByIdAsync(int id);
        Task<ResponseResult<bool>> AddAsync(CreateBranchOpeningHourDTO dto);
        Task<ResponseResult<bool>> UpdateAsync(int id, UpdateBranchOpeningHourDTO dto);
        Task<ResponseResult> DeleteAsync(int id);

        Task<ResponseResult<IEnumerable<BranchOpeningHourDTO>>> GetByBranchIdAsync(int branchId);
    }
    public class BranchOpeningHourService : IBranchOpeningHourService
    {
        private readonly IBranchOpeningHourRepository _repository;
        private readonly IBranchRepository _branchRepository;

        public BranchOpeningHourService(
            IBranchOpeningHourRepository repository,
            IBranchRepository branchRepository)
        {
            _repository = repository;
            _branchRepository = branchRepository;
        }

        public async Task<ResponseResult<IEnumerable<BranchOpeningHourDTO>>> GetAllAsync()
        {
            try
            {
                var hours = await _repository.GetAllAsync();

                if (!hours.Any())
                {
                    return ResponseResult<IEnumerable<BranchOpeningHourDTO>>.NotFound("No opening hours found");
                }

                var result = hours.Select(_MapToDto);
                return ResponseResult<IEnumerable<BranchOpeningHourDTO>>.Success(result);
            }
            catch (Exception ex)
            {
                return ResponseResult<IEnumerable<BranchOpeningHourDTO>>.FromException(ex);
            }
        }

        public async Task<ResponseResult<BranchOpeningHourDTO>> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return ResponseResult<BranchOpeningHourDTO>.Error("Invalid ID", 400);
                }

                var hour = await _repository.GetByIdAsync(id);

                if (hour == null)
                {
                    return ResponseResult<BranchOpeningHourDTO>.NotFound("Opening hour not found");
                }

                return ResponseResult<BranchOpeningHourDTO>.Success(_MapToDto(hour));
            }
            catch (Exception ex)
            {
                return ResponseResult<BranchOpeningHourDTO>.FromException(ex);
            }
        }

        public async Task<ResponseResult<IEnumerable<BranchOpeningHourDTO>>> GetByBranchIdAsync(int branchId)
        {
            try
            {
                if (branchId <= 0)
                {
                    return ResponseResult<IEnumerable<BranchOpeningHourDTO>>.Error("Invalid branch ID", 400);
                }

                var branch = await _branchRepository.GetByIdAsync(branchId);
                if (branch == null)
                {
                    return ResponseResult<IEnumerable<BranchOpeningHourDTO>>.NotFound("Branch not found");
                }

                var hours = await _repository.GetByBranchIdAsync(branchId);

                if (!hours.Any())
                {
                    return ResponseResult<IEnumerable<BranchOpeningHourDTO>>.NotFound("No opening hours found for this branch");
                }

                var result = hours.Select(_MapToDto);
                return ResponseResult<IEnumerable<BranchOpeningHourDTO>>.Success(result);
            }
            catch (Exception ex)
            {
                return ResponseResult<IEnumerable<BranchOpeningHourDTO>>.FromException(ex);
            }
        }

        public async Task<ResponseResult<bool>> AddAsync(CreateBranchOpeningHourDTO createDto)
        {
            try
            {
                if (createDto == null)
                {
                    return ResponseResult<bool>.Error("Opening hour data is required", 400);
                }

                var branch = await _branchRepository.GetByIdAsync(createDto.BranchId);
                if (branch == null)
                {
                    return ResponseResult<bool>.NotFound("Branch not found");
                }

                // Validate time if not closed
                if (!createDto.IsClosed && createDto.OpeningTime >= createDto.ClosingTime)
                {
                    return ResponseResult<bool>.Error("Opening time must be before closing time", 400);
                }

                var entity = _CreateEntityFromDto(createDto);

                var addedEntity = await _repository.AddAsync(entity);
                return ResponseResult<bool>.Created(addedEntity);
            }
            catch (Exception ex)
            {
                return ResponseResult<bool>.FromException(ex);
            }
        }

        public async Task<ResponseResult<bool>> UpdateAsync(int id, UpdateBranchOpeningHourDTO updateDto)
        {
            try
            {
                if (id <= 0)
                {
                    return ResponseResult<bool>.Error("Invalid ID", 400);
                }

                if (updateDto == null)
                {
                    return ResponseResult<bool>.Error("Update data is required", 400);
                }

                var existing = await _repository.GetByIdAsync(id);
                if (existing == null)
                {
                    return ResponseResult<bool>.NotFound("Opening hour not found");
                }

                // Apply updates
                if (updateDto.OpeningTime.HasValue) existing.OpeningTime = updateDto.OpeningTime.Value;
                if (updateDto.ClosingTime.HasValue) existing.ClosingTime = updateDto.ClosingTime.Value;
                 existing.IsClosed = updateDto.IsClosed;

                // Validate time if not closed
                if (!existing.IsClosed && existing.OpeningTime >= existing.ClosingTime)
                {
                    return ResponseResult<bool>.Error("Opening time must be before closing time", 400);
                }

                await _repository.UpdateAsync(existing);
                return ResponseResult<bool>.Success();
            }
            catch (Exception ex)
            {
                return ResponseResult<bool>.FromException(ex);
            }
        }

        public async Task<ResponseResult> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return ResponseResult.Error("Invalid ID", 400);
                }

                await _repository.DeleteAsync(id);
                return ResponseResult.Success();
            }
            catch (Exception ex)
            {
                return ResponseResult.FromException(ex);
            }
        }



        private static BranchOpeningHourDTO _MapToDto(BranchOpeningHour entity)
        {
            return new BranchOpeningHourDTO
            {
                Id = entity.Id,
                BranchId = entity.BranchId,
                DayOfWeek = entity.DayOfWeek,
                OpeningTime = entity.OpeningTime,
                ClosingTime = entity.ClosingTime,
                IsClosed = entity.IsClosed,
            };
        }

        private static BranchOpeningHour _CreateEntityFromDto(CreateBranchOpeningHourDTO dto)
        {
            return new BranchOpeningHour
            {
                BranchId = dto.BranchId,
                DayOfWeek = dto.DayOfWeek,
                OpeningTime = dto.OpeningTime,
                ClosingTime = dto.ClosingTime,
                IsClosed = dto.IsClosed,
                
            };
        }


    }
}
