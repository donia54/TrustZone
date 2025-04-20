using TrustZoneAPI.DTOs.Disabilities;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Services.Disabilities
{
    public interface IUserDisabilityService
    {
        Task<ResponseResult<List<DisabilityTypeDTO>>> GetUserDisabilitiesByUserIdAsync(string userId);
        Task<ResponseResult> SetUserDisabilityTypesAsync(string userId, List<int> disabilityTypeIds);
    }
    public class UserDisabilityService: IUserDisabilityService
    {
        private readonly IUserDisabilityRepository _userDisabilityRepository;

        public UserDisabilityService(IUserDisabilityRepository userDisabilityRepository)
        {
            _userDisabilityRepository = userDisabilityRepository;

        }


        // need to review this method
        public async Task<ResponseResult<List<DisabilityTypeDTO>>> GetUserDisabilitiesByUserIdAsync(string userId)
        {
            try
            {
                var disabilities = await _userDisabilityRepository.GetUserDisabilitiesByUserIdAsync(userId);

                if (disabilities == null || disabilities.Count == 0)
                {
                    return ResponseResult<List<DisabilityTypeDTO>>.NotFound("No disabilities found for the user.");
                }

                return ResponseResult<List<DisabilityTypeDTO>>.Success(_MapToDTOList(disabilities));
            }
            catch (Exception ex)
            {
                return ResponseResult<List<DisabilityTypeDTO>>.FromException(ex);
            }
        }


        // Set User Disability Types
        public async Task<ResponseResult> SetUserDisabilityTypesAsync(string userId, List<int> disabilityTypeIds)
        {
            try
            {
                if (disabilityTypeIds == null || disabilityTypeIds.Count == 0)
                {
                    return ResponseResult.Error("Disability type IDs cannot be empty.", 400);
                }

                await _userDisabilityRepository.SetUserDisabilityTypesAsync(userId, disabilityTypeIds);

                return ResponseResult.Success();
            }
            catch (Exception ex)
            {
                return ResponseResult.FromException(ex);
            }
        }

        private   DisabilityTypeDTO _MapToDTO(DisabilityType disabilityType)
        {
            if (disabilityType == null)
            {
                return null; // Return null if the disabilityType is null
            }

            return new DisabilityTypeDTO
            {
                Id = disabilityType.Id,
                Name = disabilityType.Name
            };
        }
        private  List<DisabilityTypeDTO> _MapToDTOList(List<DisabilityType> disabilityTypes)
        {
            if (disabilityTypes == null)
            {
                return new List<DisabilityTypeDTO>(); // Return an empty list if the input list is null
            }

            return disabilityTypes.Select(disability => _MapToDTO(disability)).ToList();
        }



    }


}
