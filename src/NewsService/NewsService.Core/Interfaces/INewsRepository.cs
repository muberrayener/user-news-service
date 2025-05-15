using NewsService.NewsService.Core.Entities;

namespace NewsService.NewsService.Core.Interfaces
{
    public interface INewsRepository
    {
        Task<IEnumerable<NewsArticleEnt>> GetAllAsync();
        Task<NewsArticleEnt> GetByIdAsync(int id);
        Task AddAsync(NewsArticleEnt newsArticle);
    }
}
