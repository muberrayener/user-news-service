using ClientApi.Core.DTOs;
using ClientApi.Core.Entities;

namespace ClientApi.Application.Interfaces
{
    public interface INewsService
    {
        Task<NewsArticleDto> RegisterNewsAsync(NewsArticleDto newsArticleDto, string token);
        Task<NewsArticle> GetNewsByIdAsync(int id, string token);
        Task<IEnumerable<NewsArticle>> GetAllNewsAsync(string token);
    }
}