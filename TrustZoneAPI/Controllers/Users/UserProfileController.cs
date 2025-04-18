using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.DTOs.Users;
using TrustZoneAPI.Services.Users;

namespace TrustZoneAPI.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserProfileController : BaseController
    {

        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService)
        {
            _userProfileService = userProfileService;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserProfile(string userId)
        {
            var response = await _userProfileService.GetUserProfileAsync(userId);
            return MapResponseToActionResult(response);
        }

      
        [HttpPut("updateInfo/{userId}")]
        public async Task<ActionResult> UpdateUserInfo(string userId, [FromBody] UpdateUserProfileDTO dto)
        {
            var response = await _userProfileService.UpdateUserInfoAsync(userId, dto);
            return MapResponseToActionResult(response);
        }

      
        [HttpPut("updatePassword/{userId}")]
        public async Task<ActionResult> UpdatePassword(string userId, [FromBody] UpdatePasswordDTO dto)
        {
            var response = await _userProfileService.UpdatePasswordAsync(userId, dto);
            return MapResponseToActionResult(response);
        }

        
        [HttpGet("generateProfilePictureUploadSas/{userId}/{fileName}")]
        public async Task<ActionResult> GenerateProfilePictureUploadSasUrl(string userId, string fileName)
        {
            var response = await _userProfileService.GenerateProfilePictureUploadSasUrlAsync(userId, fileName);
            return MapResponseToActionResult(response);
        }

      
        [HttpGet("generateCoverPictureUploadSas/{userId}/{fileName}")]
        public async Task<ActionResult> GenerateCoverPictureUploadSasUrl(string userId, string fileName)
        {
            var response = await _userProfileService.GenerateCoverPictureUploadSasUrlAsync(userId, fileName);
            return MapResponseToActionResult(response);
        }

        [HttpPut("updateProfilePicture/{userId}/{fileName}")]
        public async Task<ActionResult> UpdateProfilePicture(string userId, string fileName)
        {
            var response = await _userProfileService.UpdateProfilePictureAsync(userId, fileName);
            return MapResponseToActionResult(response);
        }


        [HttpPut("updateCoverPicture/{userId}/{fileName}")]
        public async Task<ActionResult> UpdateCoverPicture(string userId, string fileName)
        {
            var response = await _userProfileService.UpdateCoverPictureAsync(userId, fileName);
            return MapResponseToActionResult(response);
        }

        
        [HttpGet("getProfilePictureUrl/{userId}")]
        public async Task<ActionResult> GetProfilePictureUrl(string userId)
        {
            var response = await _userProfileService.GetProfilePictureUrlAsync(userId);
            return MapResponseToActionResult(response);
        }

        
        [HttpGet("getCoverPictureUrl/{userId}")]
        public async Task<ActionResult> GetCoverPictureUrl(string userId)
        {
            var response = await _userProfileService.GetCoverPictureUrlAsync(userId);
            return MapResponseToActionResult(response);
        }
    }
}
