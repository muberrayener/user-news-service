using NewsService.NewsService.Core.Entities;

namespace NewsService.NewsService.Core.Interfaces
{
    public interface INewsRepository
    {
        Task<IEnumerable<NewsArticle>> GetAllAsync();
        Task<NewsArticle> GetByIdAsync(int id);
        Task AddAsync(NewsArticle newsArticle);
    }
}
