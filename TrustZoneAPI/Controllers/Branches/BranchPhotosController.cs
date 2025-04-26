using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.DTOs.Places;
using TrustZoneAPI.Services.Places;

namespace TrustZoneAPI.Controllers.Branches
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchPhotosController : BaseController
    {

        private readonly IBranchPhotoService _branchPhotoService;

        public BranchPhotosController(IBranchPhotoService branchPhotoService)
        {
            _branchPhotoService = branchPhotoService;
        }

        [HttpGet("generate-upload-url")]
        public async Task<IActionResult> GenerateUploadUrl()
        {
            var response = await _branchPhotoService.GenerateBranchPictureUploadSasUrlAsync();
            return MapResponseToActionResult(response);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBranchPhotoDto dto)
        {
          
            var response = await _branchPhotoService.CreateAsync(CurrentUserId ,dto);
            return MapResponseToActionResult(response);
        }

        [HttpGet("branch/{branchId}")]
        public async Task<IActionResult> GetByBranchId(int branchId)
        {
            var response = await _branchPhotoService.GetPhotosByBranchIdAsync(branchId);
            return MapResponseToActionResult(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var response = await _branchPhotoService.GetByIdAsync(id);
            return MapResponseToActionResult(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _branchPhotoService.DeleteAsync(id);
            return MapResponseToActionResult(response);
        }
    }
}
