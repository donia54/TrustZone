using Microsoft.Extensions.Azure;
using TrustZoneAPI.DTOs.Places;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Services.Places
{
    public interface IFavoritePlaceService
    {
        Task<ResponseResult<IEnumerable<FavoritePlaceDto>>> GetUserFavoritePlacesAsync(string userId);

        Task<ResponseResult> AddFavoritePlaceAsync(string userId,CreateFavoritePlaceDto dto);
        Task<ResponseResult> DeleteByBranchIdAndUserIdAsync(int branchId, string UserId);
    }
    public class FavoritePlaceService : IFavoritePlaceService
    {
        private readonly IFavoritePlaceRepository _favoritePlaceRepository;
        private readonly IBranchPhotoService _branchPhotoService;
        public FavoritePlaceService(IFavoritePlaceRepository favoritePlaceRepository,IBranchPhotoService branchPhotoService)
        {
            _favoritePlaceRepository = favoritePlaceRepository;
            _branchPhotoService = branchPhotoService;
        }


        public async Task<ResponseResult<IEnumerable<FavoritePlaceDto>>> GetUserFavoritePlacesAsync(string userId)
        {
            try
            {
                var favorites = await _favoritePlaceRepository.GetFavoritePlacesByUserAsync(userId);
                var dtos = favorites.Select(_MapToDTO).ToList();
                foreach (var dto in dtos)
                {
                    var branchPhoto = await _branchPhotoService.GetMainPhotoByBranchIdAsync(dto.Branch.Id);
                    dto.BranchPhotoDto = branchPhoto.Data ?? new BranchPhotoDto();
                }
                return ResponseResult<IEnumerable<FavoritePlaceDto>>.Success(dtos);
            }
            catch (Exception ex)
            {
                return ResponseResult<IEnumerable<FavoritePlaceDto>>.FromException(ex);
            }
        }


        public async Task<ResponseResult> AddFavoritePlaceAsync(string userId,CreateFavoritePlaceDto dto)
        {
            try
            {
                var alreadyFavorited = await _favoritePlaceRepository.IsBranchFavoritedByUserAsync(userId, dto.BranchId);
                if (alreadyFavorited)
                {
                    return ResponseResult.Error("Branch already favorited by user", 400);
                }

                var favorite = new FavoritePlace
                {
                    UserId = userId,
                    BranchId = dto.BranchId,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await _favoritePlaceRepository.AddAsync(favorite);

                if (!result)
                    return ResponseResult.Error("Failed to add favorite place", 500);

                return ResponseResult.Created();
            }
            catch (Exception ex)
            {
                return ResponseResult.FromException(ex);
            }
        }

        public async Task<ResponseResult> DeleteByBranchIdAndUserIdAsync(int branchId, string UserId)
        {
            try
            {
                var deleted = await _favoritePlaceRepository.DeleteByBranchIdAndUserIdAsync(branchId, UserId);
                if (!deleted)
                    return ResponseResult.NotFound("Favorite place not found");

                return ResponseResult.Success();
            }
            catch (Exception ex)
            {
                return ResponseResult.FromException(ex);
            }
        }

        private FavoritePlaceDto _MapToDTO(FavoritePlace favoritePlace)
        {
            return new FavoritePlaceDto
            {
                Id = favoritePlace.Id,
                CreatedAt = favoritePlace.CreatedAt,
                Branch = new BranchLightDTO
                {
                    Id = favoritePlace.Branch.Id,
                    Address = favoritePlace.Branch.Address,
                    PlaceName= favoritePlace.Branch.Place?.Name ?? string.Empty
                }
            };
        }
    }
   
}
