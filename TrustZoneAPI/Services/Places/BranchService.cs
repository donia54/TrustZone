using Microsoft.AspNetCore.SignalR;
using TrustZoneAPI.DTOs.Places;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories;
using TrustZoneAPI.Repositories.Interfaces;
using TrustZoneAPI.Services.Categories;
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

        Task<ResponseResult<IEnumerable<BranchDTO>>> GetBranchesByCategoryIdAsync(int categoryId);
        Task<ResponseResult<IEnumerable<BranchLightDTO>>> FilterBranchesByFeaturesAsync(List<int> featureIds);
        Task<IEnumerable<BranchLightDTO>> SearchBranchesAsync(BranchSearchRequestDTO dto, int page, int pageSize);
        Task<ResponseResult<IEnumerable<BranchLightDTO>>> GetBranchesWithFeatureAsync(int featureId);


    }

public class BranchService : IBranchService
    {

        private readonly IBranchRepository _branchRepository;
        private readonly IPlaceService _placeService;
        public readonly ICategoryService _categoryservice;
        private readonly IHubContext<SearchHub> _searchHub;


        public BranchService(IBranchRepository branchRepository, IPlaceService placeService, ICategoryService categoryService, IHubContext<SearchHub> searchHub)
        {
            _branchRepository = branchRepository;
            _placeService = placeService;
            _categoryservice = categoryService;
            _searchHub = searchHub;
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

        public async Task<ResponseResult<IEnumerable<BranchDTO>>> GetBranchesByCategoryIdAsync(int categoryId)
        {
            var categoryExists = await  _categoryservice.IsCategoryExistsByIdAsync(categoryId);
            if (categoryExists==null)
                return ResponseResult<IEnumerable<BranchDTO>>.Error("Invalid category ID.", 400);

            var branches = await _branchRepository.GetBranchesByCategoryIdAsync(categoryId);

            var branchesDtos = branches.Select(_ConvertToDTO).ToList();

            if (!branchesDtos.Any())
                return ResponseResult<IEnumerable<BranchDTO>>.NotFound("No branches found for this category.");

            return ResponseResult<IEnumerable<BranchDTO>>.Success(branchesDtos);
        }


        public async Task<ResponseResult<IEnumerable<BranchLightDTO>>> GetBranchesWithFeatureAsync(int featureId)
        {
            var branches = await _branchRepository.GetBranchesWithFeatureAsync(featureId);
            var branchesDtos = branches.Select(_ConvertToLightDTO).ToList();

            return ResponseResult<IEnumerable<BranchLightDTO>>.Success(branchesDtos);
        }

        public async Task<IEnumerable<BranchLightDTO>> SearchBranchesAsync(BranchSearchRequestDTO dto, int page, int pageSize)
        {
            var branches = await _branchRepository.SearchBranchesAsync(dto.Query, dto.CategoryId, page, pageSize);

            var results = branches.Select(_ConvertToLightDTO).ToList();
            if (!results.Any())
                return Enumerable.Empty<BranchLightDTO>();

            await _searchHub.Clients.All.SendAsync("ReceiveSearchResults", results);

            return results;
        }


        public async Task<ResponseResult<IEnumerable<BranchLightDTO>>> FilterBranchesByFeaturesAsync(List<int> featureIds)
        {
            var branches = await _branchRepository.FilterBranchesByPlaceFeaturesAsync(featureIds);
            var branchesDtos = branches.Select(_ConvertToLightDTO);

            return ResponseResult<IEnumerable<BranchLightDTO>>.Success(branchesDtos);
        }


        private BranchLightDTO _ConvertToLightDTO(Branch branch)
        {
            return new BranchLightDTO
            {
                Id = branch.Id,
                Address = branch.Address,
                PlaceName = branch.Place?.Name ?? string.Empty

            };
        }

        private  BranchDTO _ConvertToDTO(Branch branch)
        {
            var placeDto = _placeService.GetPlaceBranchByIdAsync(branch.PlaceId).Result.Data!;
            return new BranchDTO
            {
                Id = branch.Id,
             
                // PlaceId = branch.PlaceId,
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
                Place = placeDto,


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
