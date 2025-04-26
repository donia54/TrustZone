using Humanizer;
using TrustZoneAPI.DTOs.Places;
using TrustZoneAPI.DTOs.Users;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories;
using TrustZoneAPI.Repositories.Interfaces;
using TrustZoneAPI.Services.Azure;
using static TrustZoneAPI.DTOs.Places.ReviewsDTOs;

namespace TrustZoneAPI.Services.Places
{
    public interface IReviewService
    {
 

        Task<ResponseResult<bool>> CreateReviewAsync(string userid,CreateReviewDto dto);
        Task<ResponseResult> DeleteAsync(int id);
        Task<ResponseResult> UpdateAsync(string userid, int id, UpdateReviewDto dto);
        Task<ResponseResult<IEnumerable<ReviewDto>>> GetReviewsByBranchAsync(int branchId, int page, int pageSize);
        Task<ResponseResult<IEnumerable<ReviewDto>>> GetReviewsByUserAsync(string userId);
        Task<ResponseResult<IEnumerable<ReviewDto>>> GetVerifiedReviewsAsync();
        Task<ResponseResult<ReviewDto>> VerifyReviewAsync(int id);

    }
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IBlobService _blobService; 

        public ReviewService (IReviewRepository reviewRepository,IBlobService blobService)
        {
            _reviewRepository = reviewRepository;
            _blobService = blobService;
        }


        public async Task<ResponseResult<bool>> CreateReviewAsync(string userid,CreateReviewDto dto)
        {
            var review = _MapToReview(userid,dto);

            var success = await _reviewRepository.AddAsync(review);
            return success
                ? ResponseResult<bool>.Success(true)
                : ResponseResult<bool>.Error("Failed to add review", 500);
        }

        public async Task<ResponseResult> DeleteAsync(int id)
        {
            if (id <= 0)
                return ResponseResult.Error("Invalid place ID.", 400);

            bool result = await _reviewRepository.DeleteAsync(id);
            return result ? ResponseResult.Success() : ResponseResult.NotFound("Review not found.");
        }

        public async Task<ResponseResult> UpdateAsync(string userid, int id, UpdateReviewDto dto)
        {
            if (id <= 0)
                return ResponseResult.Error("Invalid Review ID.", 400);

            var existing = await _reviewRepository.GetByIdAsync(id);
            if (existing == null)
                return ResponseResult.NotFound("Review not found.");

            
            bool result = await _reviewRepository.UpdateAsync(_MapToReview(userid,existing, dto));
            return result ? ResponseResult.Success() : ResponseResult.Error("Failed to update Review", 500);
        }

        public async Task<ResponseResult<IEnumerable<ReviewDto>>> GetReviewsByBranchAsync(int branchId, int page, int pageSize)
        {
            var reviews = await _reviewRepository.GetReviewsByBranchAsync(branchId, page, pageSize);
            var result = new List<ReviewDto>();

            foreach (var review in reviews)
            {
                var dto =  _MapToDto(review);

                result.Add(dto);
            }

            return ResponseResult<IEnumerable<ReviewDto>>.Success(result);
        }

        public async Task<ResponseResult<IEnumerable<ReviewDto>>> GetReviewsByUserAsync(string userId)
        {
            var reviews = await _reviewRepository.GetReviewsByUserAsync(userId);
            var result = reviews.Select(_MapToDto);
            return ResponseResult<IEnumerable<ReviewDto>>.Success(result);
        }
        public async Task<ResponseResult<IEnumerable<ReviewDto>>> GetVerifiedReviewsAsync()
        {
            var reviews = await _reviewRepository.GetVerifiedReviewsAsync();
            var result = reviews.Select(_MapToDto);
            return ResponseResult<IEnumerable<ReviewDto>>.Success(result);
        }

        public async Task<ResponseResult<ReviewDto>> VerifyReviewAsync(int id)
        {
            var review = await _reviewRepository.VerifyReviewAsync(id);
            if (review == null)
                return ResponseResult<ReviewDto>.NotFound("Review not found");

            return ResponseResult<ReviewDto>.Success(_MapToDto(review));
        }


   

        private Review _MapToReview(string userid,CreateReviewDto dto)
        {
            return new Review
            {
                UserId = userid,
                BranchId = dto.BranchId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                //ContentUrl = dto.ContentUrl,
                CreatedAt = DateTime.UtcNow,
                IsVerified = false
            };
        }

        private Review _MapToReview(string userid, Review existing,UpdateReviewDto dto)
        {
            existing.UserId = userid;
            existing.Rating = dto.Rating;
            existing.Comment = dto.Comment;
            // ContentUrl = dto.ContentUrl,
            existing.CreatedAt = DateTime.UtcNow;
            // IsVerified = false

            return existing;
        }



        private ReviewDto _MapToDto(Review review)
        {
            return new ReviewDto
            {
                Id = review.Id,
                user = new UserLightDTO
                {
                    Id = review.User.Id,
                    UserName = review.User.UserName,
                    ProfilePictureUrl = review.User.ProfilePicture,
                },
                BranchId = review.BranchId,
                Rating = review.Rating,
                Comment = review.Comment,
             
                CreatedAt = review.CreatedAt
            };
        }

   
    }
}
