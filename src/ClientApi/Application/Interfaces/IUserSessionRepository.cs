using ClientApi.Core.Entities;

namespace ClientApi.Application.Interfaces
{
    public interface IUserSessionRepository
    {
        Task AddSessionAsync(UserSession session);
        Task<UserSession> GetSessionAsync(string email);

        Task<string> GetTokenAsync(string email);
    }
}


