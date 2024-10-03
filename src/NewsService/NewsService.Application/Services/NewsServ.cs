
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
        public async Task<NewsArticle> AddArticleAsync(NewsArticleDto newsArticleDto)
        {
            var newsArticle = new NewsArticle
            {
                Title = newsArticleDto.Title,
                Author = newsArticleDto.Author,
                Content = newsArticleDto.Content,
                Date = GetCurrentDateTime(),
            };

            await _newsRepository.AddAsync(newsArticle);

            return newsArticle ;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllArticlesAsync() => await _newsRepository.GetAllAsync();

        public async Task<NewsArticle> GetArticleByIdAsync(int id) => await _newsRepository.GetByIdAsync(id);

        private String GetCurrentDateTime()
        {
            DateTime currentDateTime = DateTime.Now;
            string formattedDateTime = currentDateTime.ToString("yyyy-MM-dd HH:mm");
            return formattedDateTime;
        }
    }
}
