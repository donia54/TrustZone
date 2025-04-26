using TrustZoneAPI.DTOs.Places;
using TrustZoneAPI.Models;
using TrustZoneAPI.Repositories;
using TrustZoneAPI.Repositories.Interfaces;
using TrustZoneAPI.Services.Azure;
using TrustZoneAPI.Services.Users;

namespace TrustZoneAPI.Services.Places
{

    public interface IBranchPhotoService
    {

        Task<ResponseResult<BranchPhotoDto>> GetMainPhotoByBranchIdAsync(int branchId, int? preferredPhotoId = null);
        Task<ResponseResult<string>> GenerateBranchPictureUploadSasUrlAsync();
      //  Task<ResponseResult> AddBranchPictureAsync(string userId, string fileName);
      //  Task<ResponseResult<string>> GetBranchPictureUrlAsync(string userId);


        Task<ResponseResult<IEnumerable<BranchPhotoDto>>> GetPhotosByBranchIdAsync(int branchId);
        //Task<ResponseResult<BranchPhotoDto>> GetFeaturedPhotoAsync(int branchId);
        Task<ResponseResult<BranchPhotoDto>> GetByIdAsync(int id);
        Task<ResponseResult<bool>> CreateAsync(string userId,CreateBranchPhotoDto dto);
        Task<ResponseResult> DeleteAsync(int id);
    }
    public class BranchPhotoService : IBranchPhotoService
    {
        private readonly IBranchPhotoRepository _repo;
        private readonly IBlobService _blobService;
        private readonly IUserService _usersevice;
        private readonly IBranchService _branchService;

        public BranchPhotoService(IBranchPhotoRepository repo,IBlobService blobService, IUserService userservice, IBranchService branchService)
        {
            _repo = repo;
            _blobService = blobService;
            _usersevice = userservice;
            _branchService = branchService;
        }

        public async Task<ResponseResult<string>> GenerateBranchPictureUploadSasUrlAsync()
        {
            var fileName = $"{Guid.NewGuid()}";


            var sasUrl = await _blobService.GenerateUploadSasUrlAsync("review-pictures", fileName);

            return sasUrl != null
                ? ResponseResult<string>.Success(sasUrl)
                : ResponseResult<string>.Error($"Couldn't generate Branch picture upload link", 500);
        }

        public async Task<ResponseResult<IEnumerable<BranchPhotoDto>>> GetPhotosByBranchIdAsync(int branchId)
        {
            var photos = await _repo.GetPhotosByBranchIdAsync(branchId);
            var dtos = new List<BranchPhotoDto>();

            foreach (var photo in photos)
            {
                var dto = _ToDto(photo);
                dto.PhotoUrl = await _blobService.GeneratePictureLoadSasUrlAsync("review-pictures", photo.PhotoUrl);
                dtos.Add(dto);
            }

            return ResponseResult<IEnumerable<BranchPhotoDto>>.Success(dtos);
        }


        public async Task<ResponseResult<BranchPhotoDto>> GetByIdAsync(int id)
        {
            var photo = await _repo.GetByIdAsync(id);
            if (photo == null)
                return ResponseResult<BranchPhotoDto>.NotFound("Photo not found.");

            return ResponseResult<BranchPhotoDto>.Success(_ToDto(photo));
        }

        public async Task<ResponseResult<bool>> CreateAsync(string userId,CreateBranchPhotoDto dto)
        {
            try
            {

                var entity = _ToEntity(userId, dto);
                entity.PhotoUrl = _blobService.ExtractBlobName(dto.PhotoUrl);

                var result = await _repo.AddAsync(entity);

                if (!result)
                    return ResponseResult<bool>.Error("Failed to save branch photo.", 500);

                return ResponseResult<bool>.Created(true);
            }
            catch (Exception ex)
            {
                return ResponseResult<bool>.FromException(ex);
            }
        }

        public async Task<ResponseResult> DeleteAsync(int id)
        {
            var success = await _repo.DeleteAsync(id);
            return success ? ResponseResult.Success() : ResponseResult.NotFound("Photo not found.");
        }


        public async Task<ResponseResult<BranchPhotoDto>> GetMainPhotoByBranchIdAsync(int branchId, int? preferredPhotoId = null)
        {
            var photo = await _repo.GetMainPhotoByBranchIdAsync(branchId, preferredPhotoId);

            if (photo == null)
                return ResponseResult<BranchPhotoDto>.NotFound("Main photo not found.");

            var dto = _ToDto(photo);
            dto.PhotoUrl = await _blobService.GeneratePictureLoadSasUrlAsync("review-pictures", photo.PhotoUrl);

            return ResponseResult<BranchPhotoDto>.Success(dto);
        }


        public static BranchPhotoDto _ToDto(BranchPhoto entity) => new()
        {
            Id = entity.Id,
            BranchId = entity.BranchId,
            PhotoUrl = entity.PhotoUrl,
            UploadDate = entity.UploadDate,
            UploadedByUserId = entity.UploadedByUserId,
            //IsFeatured = entity.IsFeatured,
            //Caption = entity.Caption,
            //IsApproved = entity.IsApproved
        };

        public static BranchPhoto _ToEntity(string userId, CreateBranchPhotoDto dto) => new()
        {

            BranchId = dto.BranchId,
          // PhotoUrl = dto.PhotoUrl,
            UploadDate = DateTime.UtcNow,
            UploadedByUserId = userId,
            //IsFeatured = dto.IsFeatured,
            //Caption = dto.Caption,
            //IsApproved = dto.IsApproved
        };
    }
}
