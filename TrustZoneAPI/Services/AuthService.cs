using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TrustZoneAPI.DTOs;
using TrustZoneAPI.Models;
using TrustZoneAPI.Services.Misc;

namespace TrustZoneAPI.Services
{
    public interface IAuthService
    {
        Task<JwtSecurityToken> CreateJwtToken(User user);

        Task<ResponseResult<AuthDTO>> GetAuthResponseAsync(User user, string enteredPassword);
    }
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly JWT _jwt;
        private readonly RoleManager<IdentityRole> _roleManager;
        

        public AuthService(UserManager<User> userManager, IOptions<JWT> jwt, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwt = jwt.Value;
            _roleManager = roleManager;
        }


 
        public async Task<JwtSecurityToken> CreateJwtToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("Uid", user.Id.ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)

            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }


        public async Task<ResponseResult<AuthDTO>> GetAuthResponseAsync(User user, string enteredPassword)
        {

            if (user is null || !await _userManager.CheckPasswordAsync(user, enteredPassword))
            {
                return ResponseResult<AuthDTO>.Error("Login failed. Please check your credentials and try again.", 401);
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            var authModel = new AuthDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Username = user.UserName ?? string.Empty,
                ExpiresOn = jwtSecurityToken.ValidTo
            };

            return ResponseResult<AuthDTO>.Success(authModel);
        }

    }
}