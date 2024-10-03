using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClientApi.Application.Interfaces;
using ClientApi.Core.DTOs;
using ClientApi.Core.Entities;

namespace ClientApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;
        private readonly IUserSessionRepository _userSessionRepository;

        public NewsController(INewsService newsService, IUserSessionRepository userSessionRepository)
        {
            _newsService = newsService;
            _userSessionRepository = userSessionRepository;
        }

        [HttpPost]
        public async Task<ActionResult<NewsArticleDto>> RegisterNews([FromBody] NewsArticleDto newsArticleDto)
        {
            var email = User.Identity.Name;
            var token = await _userSessionRepository.GetTokenAsync(email);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is empty.");
            }
            var article = await _newsService.RegisterNewsAsync(newsArticleDto, token);
            return article;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<NewsArticle>> GetNewsById(int id)
        {
            var email = User.Identity.Name;
            var token = await _userSessionRepository.GetTokenAsync(email);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is empty.");
            }
            var article = await _newsService.GetNewsByIdAsync(id, token);
            if (article == null)
            {
                return NotFound();
            }
            return Ok(article);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsArticle>>> GetAllNews()
        {
            var email = User.Identity.Name;
            var token = await _userSessionRepository.GetTokenAsync(email);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is empty.");
            }
            var articles = await _newsService.GetAllNewsAsync(token);
            return Ok(articles);
        }
    }
}