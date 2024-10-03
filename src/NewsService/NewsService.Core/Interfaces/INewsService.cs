using NewsService.NewsService.Core.Entities;
using NewsService.NewsService.Application.DTOs;

namespace NewsService.NewsService.Core.Interfaces
{
    public interface INewsService
    {
        Task<IEnumerable<NewsArticle>> GetAllArticlesAsync();
        Task<NewsArticle> GetArticleByIdAsync(int id);
        Task<NewsArticle> AddArticleAsync(NewsArticleDto newsArticleDto);

    }
}
