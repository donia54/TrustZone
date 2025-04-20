using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using TrustZoneAPI.DTOs.Disabilities;
using TrustZoneAPI.DTOs.Users;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories.Interfaces;
using TrustZoneAPI.Services.Azure;
using TrustZoneAPI.Services.Disabilities;

namespace TrustZoneAPI.Services.Users
{
    public interface IUserProfileService
    {
        // Profile Picture Operations
        Task<ResponseResult<string>> GenerateProfilePictureUploadSasUrlAsync();
        Task<ResponseResult> UpdateProfilePictureAsync(string userId, string fileName);
        Task<ResponseResult<string>> GetProfilePictureUrlAsync(string userId);

        // Cover Picture Operations
        Task<ResponseResult<string>> GenerateCoverPictureUploadSasUrlAsync();
        Task<ResponseResult> UpdateCoverPictureAsync(string userId, string fileName);
        Task<ResponseResult<string>> GetCoverPictureUrlAsync(string userId);


        // User Info Update Operations
        Task<ResponseResult> UpdateUserInfoAsync(string userId, UpdateUserProfileDTO dto);
        Task<ResponseResult> UpdatePasswordAsync(string userId, UpdatePasswordDTO dto);
        Task<ResponseResult<UserProfileDTO>> GetUserProfileAsync(string userId);


    }
    public class UserProfileService : IUserProfileService
    {

        private readonly UserManager<User> _userManager;
        private readonly IBlobService _blobService;
        private readonly IUserRepository _userRepository;
        private readonly IUserDisabilityService _userdisability;

        public UserProfileService(UserManager<User> userManager, IBlobService blobService,IUserRepository userRepository, IUserDisabilityService userdisability)
        {
            _userManager = userManager;
            _blobService = blobService;
            _userRepository = userRepository;
            _userdisability = userdisability;
        }




        public async Task<ResponseResult<string>> GenerateProfilePictureUploadSasUrlAsync()
            => await _GenerateUploadPictureSasUrlAsync("profile");

        public async Task<ResponseResult<string>> GenerateCoverPictureUploadSasUrlAsync()
            => await _GenerateUploadPictureSasUrlAsync( "cover");

        public async Task<ResponseResult> UpdateProfilePictureAsync(string userId, string fileName)
            => await _UpdatePictureAsync(userId, fileName, "profile");

        public async Task<ResponseResult> UpdateCoverPictureAsync(string userId, string fileName)
            => await _UpdatePictureAsync(userId, fileName, "cover");

        public async Task<ResponseResult<string>> GetProfilePictureUrlAsync(string userId)
            => await _GetPictureUrlAsync(userId, "profile");

        public async Task<ResponseResult<string>> GetCoverPictureUrlAsync(string userId)
            => await _GetPictureUrlAsync(userId, "cover");



        public async Task<ResponseResult> UpdateUserInfoAsync(string userId, UpdateUserProfileDTO dto)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return ResponseResult.NotFound("User not found");


                user.UserName = dto.UserName;
                user.Email = dto.Email;
                user.Age = dto.Age.Value;

            await _userRepository.UpdateAsync(user);
            return ResponseResult.Success();
        }

        public async Task<ResponseResult> UpdatePasswordAsync(string userId, UpdatePasswordDTO dto)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
                return ResponseResult.NotFound("User not found");

            var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword, dto.NewPassword);
            if (result.Succeeded)
                return ResponseResult.Success();

            var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
            return ResponseResult.Error(errorMessages, 400);
        }


        public async Task<ResponseResult<UserProfileDTO>> GetUserProfileAsync(string userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return ResponseResult<UserProfileDTO>.NotFound("User not found");

            var profilePictureUrl = string.IsNullOrEmpty(user.ProfilePicture) ? null
                : await GenerateProfilePictureUploadSasUrlAsync();

            var coverPictureUrl = string.IsNullOrEmpty(user.CoverPicture) ? null
                : await GenerateProfilePictureUploadSasUrlAsync();   //to minimize the number of frontend calls.

            var dto = await _MapToDTO(user);



            return ResponseResult<UserProfileDTO>.Success(dto);
        }

        private async Task<ResponseResult<string>> _GenerateUploadPictureSasUrlAsync(string type)
        {
            var fileName = $"{Guid.NewGuid()}";


            var sasUrl = await _blobService.GenerateUploadSasUrlAsync(fileName);

            return sasUrl != null
                ? ResponseResult<string>.Success(sasUrl)
                : ResponseResult<string>.Error($"Couldn't generate {type} picture upload link", 500);
        }

        private async Task<ResponseResult> _UpdatePictureAsync(string userId, string fileName, string type)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return ResponseResult.NotFound("User not found");

            if (type == "profile")
                user.ProfilePicture = fileName;
            else if (type == "cover")
                user.CoverPicture = fileName;

            await _userRepository.UpdateAsync(user);
            return ResponseResult.Success();
        }

        private async Task<ResponseResult<string>> _GetPictureUrlAsync(string userId, string type)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
                return ResponseResult<string>.NotFound("User not found");

            var fileName = type == "profile" ? user.ProfilePicture : user.CoverPicture;
            if (string.IsNullOrEmpty(fileName))
                return ResponseResult<string>.NotFound($"No {type} picture found");

            var sasUrl = await _blobService.GeneratePictureLoadSasUrlAsync(fileName);
            return ResponseResult<string>.Success(sasUrl);
        }


        private async Task<List<DisabilityTypeDTO>> _GetUserDisabilityTypesAsync(string userId)
        {
            var types = await _userdisability.GetUserDisabilitiesByUserIdAsync(userId);
            if (types == null || types.Data == null || !types.Data.Any())
                return new List<DisabilityTypeDTO>();
            var disabilityTypeDTOs = types.Data.Select(t => new DisabilityTypeDTO
            {
                Id = t.Id,
                Name = t.Name
            }).ToList();

            return disabilityTypeDTOs;
        }

        private async Task _UpdateUserDisabilityTypesAsync(string userId, List<DisabilityTypeDTO> disabilityTypes)
        {
            var disabilityTypeIds = disabilityTypes.Select(d => d.Id).ToList();
            await _userdisability.SetUserDisabilityTypesAsync(userId, disabilityTypeIds);
        }



        private async Task<UserProfileDTO> _MapToDTO(User user)
        {
            var disabilityTypes = await  _GetUserDisabilityTypesAsync(user.Id);
            return new UserProfileDTO
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Age = user.Age,
                ProfilePictureUrl = user.ProfilePicture,
                CoverPictureUrl = user.CoverPicture,
                RegistrationDate = user.RegistrationDate,
                IsActive = user.IsActive,
                DisabilityTypes = disabilityTypes
            };
        }

        private bool _ValidatePictureFileType(string fileName)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".WebP", ".SVG" };
            var fileExtension = Path.GetExtension(fileName).ToLowerInvariant();
            return allowedExtensions.Contains(fileExtension);
        }
    }
}
