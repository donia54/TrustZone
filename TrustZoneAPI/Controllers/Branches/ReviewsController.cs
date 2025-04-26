using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.Services.Places;
using static TrustZoneAPI.DTOs.Places.ReviewsDTOs;

namespace TrustZoneAPI.Controllers.Branches
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : BaseController
    {

        private readonly IReviewService _reviewService;

        public ReviewsController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto dto)
        {
            var result = await _reviewService.CreateReviewAsync(CurrentUserId,dto);
            return MapResponseToActionResult(result);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var result = await _reviewService.DeleteAsync(id);
            return MapResponseToActionResult(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReview(int id, [FromBody] UpdateReviewDto dto)
        {
           
            var result = await _reviewService.UpdateAsync(CurrentUserId, id, dto);
            return MapResponseToActionResult(result);
        }

        [HttpGet("branch/{branchId}")]
        public async Task<IActionResult> GetReviewsByBranch(int branchId, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _reviewService.GetReviewsByBranchAsync(branchId, page, pageSize);
            return MapResponseToActionResult(result);
        }

        [HttpGet("user-reviews")]
        public async Task<IActionResult> GetReviewsByUser()
        {
            var result = await _reviewService.GetReviewsByUserAsync(CurrentUserId);
            return MapResponseToActionResult(result);
        }




        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet("verified")]
        public async Task<IActionResult> GetVerifiedReviews()
        {
            var result = await _reviewService.GetVerifiedReviewsAsync();
            return MapResponseToActionResult(result);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpPut("verify/{id}")]
        public async Task<IActionResult> VerifyReview(int id)
        {
            var result = await _reviewService.VerifyReviewAsync(id);
            return MapResponseToActionResult(result);
        }

    }
}
