using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.DTOs.Places;
using TrustZoneAPI.Services.Places;

namespace TrustZoneAPI.Controllers.Branches
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FavoritePlaceController : BaseController
    {
        private readonly IFavoritePlaceService _favoritePlaceService;

        public FavoritePlaceController(IFavoritePlaceService favoritePlaceService)
        {
            _favoritePlaceService = favoritePlaceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserFavoritePlaces()
        {
            var response = await _favoritePlaceService.GetUserFavoritePlacesAsync(CurrentUserId);
            return MapResponseToActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> AddFavoritePlace([FromBody] CreateFavoritePlaceDto dto)
        {
           
            var response = await _favoritePlaceService.AddFavoritePlaceAsync( CurrentUserId ,dto);
            return MapResponseToActionResult(response);
        }

        [HttpDelete("{branchId}")]
        public async Task<IActionResult> RemoveFavoritePlaceByBranchID(int branchId)
        {
            var response = await _favoritePlaceService.DeleteByBranchIdAndUserIdAsync(branchId,CurrentUserId);
            return MapResponseToActionResult(response);
        }
    }
}
