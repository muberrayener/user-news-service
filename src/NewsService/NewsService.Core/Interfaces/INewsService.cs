using NewsService.NewsService.Core.Entities;
using NewsService.NewsService.Application.DTOs;

namespace NewsService.NewsService.Core.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsArticleEnt>> GetAllArticlesAsync();
        Task<NewsArticleEnt> GetArticleByIdAsync(int id);
        Task<NewsArticleEnt> AddArticleAsync(NewsArticleDto newsArticleDto);

    }
}
