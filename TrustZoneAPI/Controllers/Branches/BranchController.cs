using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.DTOs.Places;
using TrustZoneAPI.Services.Places;

namespace TrustZoneAPI.Controllers.Places
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : BaseController
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchDTO>>> GetAll()
        {
            var result = await _branchService.GetAllAsync();
            return MapResponseToActionResult(result);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<BranchDTO>> GetById(int id)
        {
            var result = await _branchService.GetByIdAsync(id);
            return MapResponseToActionResult(result);
        }

        [HttpPost]
        public async Task<ActionResult<BranchDTO>> Create([FromBody] CreateBranchDTO createDto)
        {
            var result = await _branchService.AddAsync(createDto);
            return MapResponseToActionResult(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] UpdateBranchDTO updateDto)
        {
            var result = await _branchService.UpdateAsync(id, updateDto);
            return MapResponseToActionResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await _branchService.DeleteAsync(id);
            return MapResponseToActionResult(result);
        }

    }
}
