using UserService.UserService.Core.Entities;

namespace UserService.UserService.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllAsync();
        Task<User> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task<User> GetByEmailAsync(string email);
    }
}
