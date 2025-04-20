using Microsoft.AspNetCore.Identity;
using TrustZoneAPI.DTOs;
using TrustZoneAPI.DTOs.User;
using TrustZoneAPI.DTOs.Users;
using TrustZoneAPI.Models;
using TrustZoneAPI.Services.Repositories.Interfaces;

namespace TrustZoneAPI.Services.Users
{
    public interface IUserService
    {
        Task<ResponseResult<AuthDTO>> RegisterUserAsync(RegistrUserIdentity model);
        Task<ResponseResult<AuthDTO>> LoginAsync(LoginDTO login);
        Task<User?> GetByIdAsync(string id);
        bool IsCurrentUser(string userId);
        Task<bool> IsUserExists(string userId);
        string GetCurrentUserId();


        Task<ResponseResult<IEnumerable<UserDTO>>> GetAllUsersAsync();
        Task<ResponseResult<UserDTO>> GetUserByIdAsync(string id);
        Task<ResponseResult<UserDTO>> GetUserByEmailAsync(string email);
        Task<ResponseResult> DeleteUserAsync(string id);
    }
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _UserManager;
        private readonly ITransactionService _TransactionService;
        private readonly IAuthService _AuthService;
        private readonly SignInManager<User> _SignInManager;
        private readonly IUserRepository _UserRepository;
        IHttpContextAccessor _HttpContextAccessor;

        public UserService(UserManager<User> userManager,ITransactionService transactionService,
            IAuthService authService, SignInManager<User> signInManager,IUserRepository userRepository
            , IHttpContextAccessor httpContextAccessor)
        {
            _UserManager = userManager;
            _TransactionService = transactionService;
            _AuthService = authService;
            _SignInManager = signInManager;
            _UserRepository = userRepository;
            _HttpContextAccessor = httpContextAccessor;

            _userRepository = userRepository;
        }
        public bool IsCurrentUser(string userId)
        {
            string CurrentUserId = _HttpContextAccessor.HttpContext?.Items["UserId"] as string ?? string.Empty;
            return CurrentUserId == userId;
        }
        public string GetCurrentUserId()
        {
            return _HttpContextAccessor.HttpContext?.Items["UserId"] as string ?? string.Empty;
        }

        public async Task<ResponseResult<AuthDTO>> RegisterUserAsync(RegistrUserIdentity model)
        {
            try
            {
                var existingUserByEmail = await _UserManager.FindByEmailAsync(model.Email);
                if (existingUserByEmail != null)
                {
                    return ResponseResult<AuthDTO>.Error("Email is already registered.", 400);
                }

                var user = _InitializeUserRegisterationAsync(model);
                var result = await _UserManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    var errors = result.Errors.Select(e => e.Description).ToList();
                    return ResponseResult<AuthDTO>.Error(errors, 400);
                }

           
                //here i will manage the roles topic int the future 

                var RolesAdded = await _UserManager.AddToRoleAsync(user, "User");
                if (!RolesAdded.Succeeded)
                {
                    return ResponseResult<AuthDTO>.Error($"Failed to add role to user", 500);
                }
                var AuthTokenResult = await _AuthService.GetAuthResponseAsync(user, model.Password);
                if (AuthTokenResult.IsSuccess && AuthTokenResult.Data != null)
                {
                    return ResponseResult<AuthDTO>.Created(AuthTokenResult.Data);
                }
                else
                {
                    return ResponseResult<AuthDTO>.Error($"Registration failed: {AuthTokenResult.ErrorMessage}", AuthTokenResult.StatusCode);
                }


            }
            catch (Exception ex)
            {
                var innerException = ex.InnerException != null ? ex.InnerException.Message : "No inner exception";
                return ResponseResult<AuthDTO>.Error($"Registration failed: {ex.Message}. Inner: {innerException}", 500);
            }
        }

        public async Task<ResponseResult<AuthDTO>> LoginAsync(LoginDTO login)
        {
           var user= await _UserManager.FindByEmailAsync(login.Email);
           if (user == null) {
                return ResponseResult<AuthDTO>.Error("Invalid email or password", 400);
            }
            var signInResult = await _SignInManager.PasswordSignInAsync(user.UserName, login.Password, true, true);
            if (!signInResult.Succeeded)
            {
                return ResponseResult<AuthDTO>.Error("Invalid email or password", 400);
            }
            var AuthTokenResult = await _AuthService.GetAuthResponseAsync(user, login.Password);

            if (AuthTokenResult.IsSuccess && AuthTokenResult.Data != null)
               return ResponseResult<AuthDTO>.Success(AuthTokenResult.Data);
            else
               return AuthTokenResult; 
           
        }




        public async Task<ResponseResult<IEnumerable<UserDTO>>> GetAllUsersAsync()
        {
            try
            {
                var users = await _userRepository.GetAllAsync();
                var userDtos = users.Select(u => _MapToDto(u)).ToList();
                return ResponseResult<IEnumerable<UserDTO>>.Success(userDtos);
            }
            catch (Exception ex)
            {
                return ResponseResult<IEnumerable<UserDTO>>.FromException(ex);
            }
        }

        public async Task<ResponseResult<UserDTO>> GetUserByIdAsync(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
                return ResponseResult<UserDTO>.NotFound("User not found.");

            return ResponseResult<UserDTO>.Success(_MapToDto(user));
        }

        public async Task<ResponseResult<UserDTO>> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
                return ResponseResult<UserDTO>.NotFound("User not found.");

            return ResponseResult<UserDTO>.Success(_MapToDto(user));
        }

        public async Task<ResponseResult> DeleteUserAsync(string id)
        {
            var result = await _userRepository.DeleteAsync(id);
            if (!result)
                return ResponseResult.Error("Unable to delete user", 400);

            return ResponseResult.Success();
        }




        private User _InitializeUserRegisterationAsync(RegistrUserIdentity model)
        {
            return new User
            {
                Id = Guid.NewGuid().ToString(),
                UserName = model.UserName,
                Email = model.Email,
                Age = model.Age,
                RegistrationDate = DateTime.UtcNow,
                IsActive = true
            };
        }

        public Task<User?> GetByIdAsync(string id)
        {
            return _UserRepository.GetByIdAsync(id);
        }

        public async Task<bool> IsUserExists(string userId)
        {
            return await _UserRepository.IsUserExists(userId);
        }
        private static UserDTO _MapToDto(User user)
        {
            return new UserDTO
            {
                Id = user.Id,
                Email = user.Email!,
                Age = user.Age,
                ProfilePicture = user.ProfilePicture,
                CoverPicture = user.CoverPicture,
                RegistrationDate = user.RegistrationDate,
                IsActive = user.IsActive
            };
        }



    }
}

