using TrustZoneAPI.DTOs.AccessibilityFeatures;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Services.Places
{

    public interface IPlaceFeatureService
    {
        Task<ResponseResult<bool>> AddFeatureToPlaceAsync(AddFeatureToPlaceDTO dto);
        Task<ResponseResult<bool>> RemoveFeatureFromPlaceAsync(AddFeatureToPlaceDTO dto);
        Task<ResponseResult<List<AccessibilityFeatureDTO>>> GetFeaturesByPlaceIdAsync(int placeId);
        Task<ResponseResult<bool>> AddFeatureListToPlaceAsync(AddFeatureListToPlaceDTO dto);

    }
    public class PlaceFeatureService : IPlaceFeatureService
    {
        private readonly IPlaceFeatureRepository _repository;
       // private readonly IPlaceService _placeService;

        public PlaceFeatureService(IPlaceFeatureRepository repository)
        {
            _repository = repository;
           // _placeService = placeService;
        }

        public async Task<ResponseResult<bool>> AddFeatureToPlaceAsync(AddFeatureToPlaceDTO dto)
        {
            

            var exists = await _repository.IsFeatureExistsAsync(dto.PlaceId, dto.FeatureId);
            if (exists)
                return ResponseResult<bool>.Error("Feature already exists for this place", 400);

            var entity = new PlaceFeature
            {
                PlaceId = dto.PlaceId,
                FeatureId = dto.FeatureId
            };

            var result = await _repository.AddFeatureToPlaceAsync(entity);
            if (!result)
                return ResponseResult<bool>.Error("Failed to add feature to place", 500);

            return ResponseResult<bool>.Created(true);
        }

        public async Task<ResponseResult<bool>> RemoveFeatureFromPlaceAsync(AddFeatureToPlaceDTO dto)
        {
            var exists = await _repository.IsFeatureExistsAsync(dto.PlaceId, dto.FeatureId);
            if (!exists)
                return ResponseResult<bool>.NotFound("Feature not found for this place");

            var entity = new PlaceFeature
            {
                PlaceId = dto.PlaceId,
                FeatureId = dto.FeatureId
            };

            var result = await _repository.RemoveFeatureFromPlaceAsync(entity);
            if (!result)
                return ResponseResult<bool>.Error("Failed to remove feature from place", 500);

            return ResponseResult<bool>.Success(true);
        }

        public async Task<ResponseResult<List<AccessibilityFeatureDTO>>> GetFeaturesByPlaceIdAsync(int placeId)
        {
            var features = await _repository.GetFeaturesByPlaceIdAsync(placeId);
            var dtoList = features.Select(f => new AccessibilityFeatureDTO
            {
                FeatureId = f.Feature.FeatureId,
                FeatureName = f.Feature.FeatureName,
                Description = f.Feature.Description
            }).ToList();

            return ResponseResult<List<AccessibilityFeatureDTO>>.Success(dtoList);
        }


        public async Task<ResponseResult<bool>> AddFeatureListToPlaceAsync(AddFeatureListToPlaceDTO dto)
        {
            var failedFeatures = new List<int>();

            foreach (var featureId in dto.FeatureIds)
            {
                bool exists = await _repository.IsFeatureExistsAsync(dto.PlaceId, featureId);
                if (!exists)
                {
                    var placeFeature = new PlaceFeature
                    {
                        PlaceId = dto.PlaceId,
                        FeatureId = featureId
                    };

                    var result = await _repository.AddFeatureToPlaceAsync(placeFeature);
                    if (!result)
                        failedFeatures.Add(featureId);
                }
            }

            if (failedFeatures.Count == dto.FeatureIds.Count)
                return ResponseResult<bool>.Error("Failed to add any features", 500);

            if (failedFeatures.Count > 0)
                return ResponseResult<bool>.Error(
                    $"Some features failed to be added: {string.Join(", ", failedFeatures)}", 207); // 207: Multi-Status

            return ResponseResult<bool>.Created(true);
        }


    }
}
