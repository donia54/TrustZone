using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetUserProfile()
        {
            var response = await _userProfileService.GetUserProfileAsync(CurrentUserId);
            return MapResponseToActionResult(response);
        }
        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserProfileByUserId(string userId)
        {
            var response = await _userProfileService.GetUserProfileAsync(userId);
            return MapResponseToActionResult(response);
        }


        [Authorize]
        [HttpPut]
        public async Task<ActionResult> UpdateUserInfo( [FromBody] UpdateUserProfileDTO dto)
        {
            var response = await _userProfileService.UpdateUserInfoAsync(CurrentUserId, dto);
            return MapResponseToActionResult(response);
        }


        [Authorize]
        [HttpPut ("password")]
        public async Task<ActionResult> UpdatePassword( [FromBody] UpdatePasswordDTO dto)
        {
            var response = await _userProfileService.UpdatePasswordAsync(CurrentUserId, dto);
            return MapResponseToActionResult(response);
        }


       // [Authorize]
        [HttpGet("generateProfilePictureUploadSas")]
        public async Task<ActionResult> GenerateProfilePictureUploadSasUrl()
        {
            var response = await _userProfileService.GenerateProfilePictureUploadSasUrlAsync();
            return MapResponseToActionResult(response);
        }


       // [Authorize]
        [HttpGet("generateCoverPictureUploadSas")]
        public async Task<ActionResult> GenerateCoverPictureUploadSasUrl()
        {
            var response = await _userProfileService.GenerateCoverPictureUploadSasUrlAsync();
            return MapResponseToActionResult(response);
        }


        [Authorize]
        [HttpPut("ProfilePicture/{fileName}")]
        public async Task<ActionResult> UpdateProfilePicture(string fileName)
        {
            var response = await _userProfileService.UpdateProfilePictureAsync(CurrentUserId, fileName);
            return MapResponseToActionResult(response);
        }


        [Authorize]
        [HttpPut("CoverPicture/{fileName}")]
        public async Task<ActionResult> UpdateCoverPicture( string fileName)
        {
            var response = await _userProfileService.UpdateCoverPictureAsync(CurrentUserId, fileName);
            return MapResponseToActionResult(response);
        }


        [Authorize]
        [HttpGet("profile-picture")]
        public async Task<ActionResult> GetProfilePictureUrl()
        {
            var response = await _userProfileService.GetProfilePictureUrlAsync(CurrentUserId);
            return MapResponseToActionResult(response);
        }


        [Authorize]
        [HttpGet("cover-picture")]
        public async Task<ActionResult> GetCoverPictureUrl()
        {
            var response = await _userProfileService.GetCoverPictureUrlAsync(CurrentUserId);
            return MapResponseToActionResult(response);
        }
    }
}
