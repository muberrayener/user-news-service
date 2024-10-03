using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ClientApi.Application.Interfaces;
using ClientApi.Core.Entities;
using ClientApi.Core.DTOs;

namespace ClientApi.Application.Services
{
    public class NewsService : INewsService
    {
        private readonly HttpClient _httpClient;

        public NewsService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<NewsArticle>> GetAllNewsAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<IEnumerable<NewsArticle>>("http://localhost:5005/news");
        }

        public async Task<NewsArticleDto> RegisterNewsAsync(NewsArticleDto newsArticleDto, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5005/news/register", newsArticleDto);
            response.EnsureSuccessStatusCode();
            return newsArticleDto;
        }
        public async Task<NewsArticle> GetNewsByIdAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return await _httpClient.GetFromJsonAsync<NewsArticle>($"http://localhost:5005/news/{id}");
        }
    }
}