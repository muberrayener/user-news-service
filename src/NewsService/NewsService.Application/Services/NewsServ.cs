
using NewsService.NewsService.Application.DTOs;
using NewsService.NewsService.Core.Entities;
using NewsService.NewsService.Core.Interfaces;

namespace NewsService.NewsService.Application.Services
{
    public class NewsServ : INewsService
    {
        private readonly INewsRepository _newsRepository;

        public NewsServ(INewsRepository newsRepository) 
        {
            _newsRepository = newsRepository;
        }
        public async Task<NewsArticleEnt> AddArticleAsync(NewsArticleDto newsArticleDto)
        {
            var newsArticle = new NewsArticleEnt
            {
                Title = newsArticleDto.Title,
                Author = newsArticleDto.Author,
                Content = newsArticleDto.Content,
                insert_date = DateTime.UtcNow,
            };

            await _newsRepository.AddAsync(newsArticle);

            return newsArticle ;
        }

        public async Task<IEnumerable<NewsArticleEnt>> GetAllArticlesAsync() => await _newsRepository.GetAllAsync();

        public async Task<NewsArticleEnt> GetArticleByIdAsync(int id) => await _newsRepository.GetByIdAsync(id);

        
    }
}
