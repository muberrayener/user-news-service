using System.IdentityModel.Tokens.Jwt;
using UserService.UserService.Core.DTOs;
using UserService.UserService.Core.Entities;

namespace UserService.UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id);
        Task<User> RegisterUserAsync(UserRegistrationDto userRegistrationDto);
        Task<string> LoginUserAsync(UserLoginDto userLoginDto);
        Task EnsureAdminUserExistsAsync();

    }
}
