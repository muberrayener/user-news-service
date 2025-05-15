using UserService.UserService.Core.Entities;

namespace UserService.UserService.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserEnt>> GetAllAsync();
        Task<UserEnt> GetByIdAsync(int id);
        Task AddAsync(UserEnt user);
        Task<UserEnt> GetByEmailAsync(string email);
    }
}
