using NuGet.Protocol.Core.Types;
using TrustZoneAPI.DTOs.Disabilities;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;

namespace TrustZoneAPI.Services.Disabilities
{
    public interface IUserDisabilityService
    {
        Task<ResponseResult<List<UserDisabilityDto>>> GetAllAsync();
        Task<ResponseResult<UserDisabilityDto>> GetByIdAsync(int id);
        Task<ResponseResult> AddAsync(UserDisabilityCreateDTO dto);
        Task<ResponseResult> DeleteAsync(int id);
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



        public async Task<ResponseResult<List<UserDisabilityDto>>> GetAllAsync()
        {
            var data = await _userDisabilityRepository.GetAllAsync();

            var result = data.Select(_MapToDTO).ToList();

            return ResponseResult<List<UserDisabilityDto>>.Success(result);
        }


        public async Task<ResponseResult<UserDisabilityDto>> GetByIdAsync(int id)
        {
            var entity = await _userDisabilityRepository.GetByIdAsync(id);
            if (entity == null)
                return ResponseResult<UserDisabilityDto>.NotFound("User disability not found.");

            var dto = _MapToDTO(entity);

            return ResponseResult<UserDisabilityDto>.Success(dto);
        }

        public async Task<ResponseResult> AddAsync(UserDisabilityCreateDTO dto)
        {
            try
            {
           

                if (!await _disabilityTypeService.IsDisabilityTypeExistsAsync(dto.DisabilityTypeId))
                    return ResponseResult.NotFound("Disability type not found.");

                var userDisability = new UserDisability
                {
                    UserId = dto.UserId,
                    DisabilityTypeId = dto.DisabilityTypeId
                };

                var result = await _userDisabilityRepository.AddAsync(userDisability);

                return result
                    ? ResponseResult.Created()
                    : ResponseResult.Error("Failed to add user disability.", 500);
            }
            catch (Exception ex)
            {
                return ResponseResult.FromException(ex);
            }
        }


        public async Task<ResponseResult> DeleteAsync(int id)
        {
            var result = await _userDisabilityRepository.DeleteAsync(id);
            return result
                ? ResponseResult.Success()
                : ResponseResult.NotFound("User disability not found.");
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


        private UserDisabilityDto _MapToDTO(UserDisability userDisability)
        {
    
            return new UserDisabilityDto
            {
                Id = userDisability.Id,
                DisabilityTypeId = userDisability.DisabilityTypeId,
                UserId = userDisability.UserId,
                DisabilityTypeName = userDisability.DisabilityType.Name
            };
        }
        private   DisabilityTypeDTO _MapToDTO(DisabilityType disabilityType)
        {
        
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
                return new List<DisabilityTypeDTO>(); 
            }

            return disabilityTypes.Select(disability => _MapToDTO(disability)).ToList();
        }



    }


}
