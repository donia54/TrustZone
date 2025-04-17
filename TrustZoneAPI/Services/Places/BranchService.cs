using TrustZoneAPI.DTOs.Places;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;
using static TrustZoneAPI.DTOs.Places.ReviewsDTOs;

namespace TrustZoneAPI.Services.Places
{
    public interface IBranchService
    {

        Task<ResponseResult<IEnumerable<BranchDTO>>> GetAllAsync();
        Task<ResponseResult<BranchDTO>> GetByIdAsync(int id);
        Task<ResponseResult<BranchDTO>> AddAsync(CreateBranchDTO createDto);
        Task<ResponseResult> UpdateAsync(int id, UpdateBranchDTO updateDto);
        Task<ResponseResult> DeleteAsync(int id);
       // Task<ResponseResult<IEnumerable<BranchDTO>>> GetBranchesByPlaceAsync(int placeId);
    }

public class BranchService : IBranchService
    {

        private readonly IBranchRepository _branchRepository;

        public BranchService(IBranchRepository branchRepository)
        {
            _branchRepository = branchRepository;
        }

        public async Task<ResponseResult<IEnumerable<BranchDTO>>> GetAllAsync()
        {
            var branches = await _branchRepository.GetAllAsync();

            if (!branches.Any())
                return ResponseResult<IEnumerable<BranchDTO>>.NotFound("No branches found");

            var branchDtos = branches.Select(_ConvertToDTO);

            return ResponseResult<IEnumerable<BranchDTO>>.Success(branchDtos);
        }

        public async Task<ResponseResult<BranchDTO>> GetByIdAsync(int id)
        {
            if (id <= 0)
                return ResponseResult<BranchDTO>.Error("Invalid branch ID", 400);

            var branch = await _branchRepository.GetByIdAsync(id);
            if (branch == null)
                return ResponseResult<BranchDTO>.NotFound("Branch not found");

            return ResponseResult<BranchDTO>.Success(_ConvertToDTO(branch));
        }

        public async Task<ResponseResult<BranchDTO>> AddAsync(CreateBranchDTO dto)
        {
            var branch = _ConvertToEntity(dto);
            if (!await _branchRepository.AddAsync(branch))
                return ResponseResult<BranchDTO>.Error("Failed to add branch", 500);
            return ResponseResult<BranchDTO>.Created(_ConvertToDTO(branch));
        }

        public async Task<ResponseResult> UpdateAsync(int id, UpdateBranchDTO dto)
        {
            if (id <= 0)
                return ResponseResult.Error("Invalid branch ID", 400);

            var existingBranch = await _branchRepository.GetByIdAsync(id);
            if (existingBranch == null)
                return ResponseResult.NotFound("Branch not found");

            _UpdateEntity(existingBranch, dto);
            var result = await _branchRepository.UpdateAsync(existingBranch);

            return result ? ResponseResult.Success() : ResponseResult.Error("Failed to update branch", 500);
        }

        public async Task<ResponseResult> DeleteAsync(int id)
        {
            if (id <= 0)
                return ResponseResult.Error("Invalid branch ID", 400);

            var result = await _branchRepository.DeleteAsync(id);
            return result ? ResponseResult.Success() : ResponseResult.NotFound("Branch not found");
        }


        private static BranchDTO _ConvertToDTO(Branch branch)
        {
            return new BranchDTO
            {
                Id = branch.Id,
                PlaceId = branch.PlaceId,
                Address = branch.Address,
                Website = branch.Website,
                Phone = branch.Phone,
                CreatedAt = branch.CreatedAt,

                OpiningHours = branch.BranchOpeningHours?.Select(hour => new BranchOpeningHourDTO
                {
                    Id = hour.Id,
                    BranchId = hour.BranchId,
                    DayOfWeek = hour.DayOfWeek,
                    OpeningTime = hour.OpeningTime,
                    ClosingTime = hour.ClosingTime

                }).ToList(),

   
            };
        }

        private static Branch _ConvertToEntity(CreateBranchDTO dto)
        {
            return new Branch
            {
                PlaceId = dto.PlaceId,
                Address = dto.Address,
                Website = dto.Website,
                Phone = dto.Phone,
                CreatedAt = DateTime.UtcNow
            };
        }

        private static void _UpdateEntity(Branch branch, UpdateBranchDTO dto)
        {
            branch.Address = dto.Address;
            branch.Website = dto.Website;
            branch.Phone = dto.Phone;
        }
    }
}
