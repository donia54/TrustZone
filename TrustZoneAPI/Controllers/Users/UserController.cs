using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrustZoneAPI.DTOs.User;
using TrustZoneAPI.DTOs.Users;
using TrustZoneAPI.Services.Users;

namespace TrustZoneAPI.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;

        }



        [HttpPost("register-user")]
        public async Task<ActionResult> Register([FromBody] RegistrUserIdentity model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.RegisterUserAsync(model);
            return MapResponseToActionResult(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.LoginAsync(model);
            return MapResponseToActionResult(result);
        }

    }
}
