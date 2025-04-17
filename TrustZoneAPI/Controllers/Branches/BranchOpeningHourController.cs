using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.DTOs.Places;
using TrustZoneAPI.Services.Places;

namespace TrustZoneAPI.Controllers.Branches
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchOpeningHourController : BaseController
    {


        private readonly IBranchOpeningHourService _branchOpeningHourService;

        public BranchOpeningHourController(IBranchOpeningHourService branchOpeningHourService)
        {
            _branchOpeningHourService = branchOpeningHourService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchOpeningHourDTO>>> GetAll()
        {
            var result = await _branchOpeningHourService.GetAllAsync();
            return MapResponseToActionResult(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BranchOpeningHourDTO>> GetById(int id)
        {
            var result = await _branchOpeningHourService.GetByIdAsync(id);
            return MapResponseToActionResult(result);
        }


        [HttpPost]
        public async Task<ActionResult<bool>> Create([FromBody] CreateBranchOpeningHourDTO createDto)
        {
            var result = await _branchOpeningHourService.AddAsync(createDto);
            return MapResponseToActionResult(result);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<bool>> Update(int id, [FromBody] UpdateBranchOpeningHourDTO updateDto)
        {
            var result = await _branchOpeningHourService.UpdateAsync(id, updateDto);
            return MapResponseToActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _branchOpeningHourService.DeleteAsync(id);
            return MapResponseToActionResult(result);
        }


        [HttpGet("by-branch/{branchId}")]
        public async Task<ActionResult<IEnumerable<BranchOpeningHourDTO>>> GetByBranchId(int branchId)
        {
            var result = await _branchOpeningHourService.GetByBranchIdAsync(branchId);
            return MapResponseToActionResult(result);
        }

    }
}
