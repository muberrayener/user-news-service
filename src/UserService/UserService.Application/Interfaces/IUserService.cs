using System.IdentityModel.Tokens.Jwt;
using UserService.UserService.Core.DTOs;
using UserService.UserService.Core.Entities;

namespace UserService.UserService.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserEnt>> GetAllUsersAsync();
        Task<UserEnt> GetUserByIdAsync(int id);
        Task<UserEnt> RegisterUserAsync(UserRegistrationDto userRegistrationDto);
        Task<string> LoginUserAsync(UserLoginDto userLoginDto);

    }
}
