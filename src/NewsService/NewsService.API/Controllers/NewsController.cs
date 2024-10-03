

using Microsoft.AspNetCore.Mvc;
using NewsService.NewsService.Core.Interfaces;
using NewsService.NewsService.Application.DTOs;
using NewsService.NewsService.Core.Entities;
using Microsoft.AspNetCore.Authorization;

namespace NewsService.NewsService.API.Controllers
    {
        
        [Route("api/[controller]")]
        [ApiController]
        public class NewsController : ControllerBase
        {
            private readonly INewsService _newsService;

            public NewsController(INewsService newsService)
            {
                _newsService = newsService;
            }

            [Authorize]
            [HttpGet]
            public async Task<ActionResult<IEnumerable<NewsArticle>>> GetAllArticles()
            {
                var newsArticles = await _newsService.GetAllArticlesAsync();
                return Ok(newsArticles);
            }

            [Authorize]
            [HttpGet("{id}")]
            public async Task<ActionResult<NewsArticle>> GetArticleById(int id)
            {
                var newsArticle = await _newsService.GetArticleByIdAsync(id);
                if (newsArticle == null) return NotFound();
                return Ok(newsArticle);
            }

            [Authorize(Policy = "AdminOnly")]
            [HttpPost("register")]
            public async Task<ActionResult<NewsArticle>>AddArticle([FromBody] NewsArticleDto newsArticleDto)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var newsArticle = await _newsService.AddArticleAsync(newsArticleDto);
                return CreatedAtAction(nameof(AddArticle), new { id = newsArticle.Id }, newsArticle);
            }

        }
    }

