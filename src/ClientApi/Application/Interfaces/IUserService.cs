using System.Threading.Tasks;
using ClientApi.Core.DTOs;

namespace ClientApi.Application.Interfaces
{
    public interface IUserService
    {
        Task<string> LoginAsync(LoginRequest request);
        Task<UserDto> RegisterAsync(RegisterRequest request);
        Task<IEnumerable<UserDto>> GetAllUsersAsync(string token);
        Task<UserDto> GetUserByIdAsync(int id, string token);
    }
}