using TrustZoneAPI.DTOs.AccessibilityFeatures;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;


namespace TrustZoneAPI.Services.AccessibilityFeatures
{

    public interface IAccessibilityFeatureService
    {
        Task<ResponseResult<IEnumerable<AccessibilityFeatureDTO>>> GetAllAsync();
        Task<ResponseResult<AccessibilityFeatureDTO>> GetByIdAsync(int id);
        Task<ResponseResult> AddAsync(CreateAccessibilityFeatureDTO dto);
        Task<ResponseResult> UpdateAsync(int FeatureId, CreateAccessibilityFeatureDTO dto);
        Task<ResponseResult> DeleteAsync(int id);
        Task<ResponseResult<IEnumerable<AccessibilityFeatureDTO>>> SearchAsync(string keyword);
    }
    public class AccessibilityFeatureService : IAccessibilityFeatureService
    {

        private readonly IAccessibilityFeatureRepository _repository;

        public AccessibilityFeatureService(IAccessibilityFeatureRepository repository)
        {
            _repository = repository;
        }


        public async Task<ResponseResult<IEnumerable<AccessibilityFeatureDTO>>> GetAllAsync()
        {
            var features = await _repository.GetAllAsync();
            var result = features.Select(_MapToDto);
            return ResponseResult<IEnumerable<AccessibilityFeatureDTO>>.Success(result);
        }

        public async Task<ResponseResult<AccessibilityFeatureDTO>> GetByIdAsync(int id)
        {
            var feature = await _repository.GetByIdAsync(id);
            if (feature == null)
                return ResponseResult<AccessibilityFeatureDTO>.NotFound("Feature not found");

            return _MapToDto(feature);
        }

        public async Task<ResponseResult> AddAsync(CreateAccessibilityFeatureDTO dto)
        {
            var newFeature = new AccessibilityFeature
            {
                FeatureName = dto.FeatureName,
                Description = dto.Description
            };

            var success = await _repository.AddAsync(newFeature);
            if (!success)
                return ResponseResult.Error("Failed to create", 500);

            return ResponseResult.Created();
        }


        public async Task<ResponseResult> UpdateAsync(int FeatureId, CreateAccessibilityFeatureDTO dto)
        {
            var feature = await _repository.GetByIdAsync(FeatureId);
            if (feature == null)
                return ResponseResult.NotFound("Feature not found");

            feature.FeatureName = dto.FeatureName;
            feature.Description = dto.Description;

            var success = await _repository.UpdateAsync(feature);
            if (!success)
                return ResponseResult.Error("Failed to update", 500);

            return ResponseResult.Success();
        }


        public async Task<ResponseResult> DeleteAsync(int id)
        {
            var success = await _repository.DeleteAsync(id);
            return success ? ResponseResult.Success() : ResponseResult.NotFound("Feature not found");
        }


        public async Task<ResponseResult<IEnumerable<AccessibilityFeatureDTO>>> SearchAsync(string keyword)
        {
            var features = await _repository.SearchByNameAsync(keyword);
            var result = features.Select(_MapToDto);
           return ResponseResult<IEnumerable<AccessibilityFeatureDTO>>.Success(result);
        }



        private AccessibilityFeatureDTO _MapToDto(AccessibilityFeature feature)
        {
            return new AccessibilityFeatureDTO
            {
                FeatureId = feature.FeatureId,
                FeatureName = feature.FeatureName,
                Description = feature.Description
            };
        }

    }
}
