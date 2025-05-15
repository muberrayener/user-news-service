

using Microsoft.AspNetCore.Mvc;
using NewsService.NewsService.Core.Interfaces;
using NewsService.NewsService.Application.DTOs;
using NewsService.NewsService.Core.Entities;
using Microsoft.AspNetCore.Authorization;
using NewsService.NewsService.Application.Mapping;


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
            public async Task<ActionResult<IEnumerable<NewsArticleEnt>>> GetAllArticles()
            {
                var newsArticles = await _newsService.GetAllArticlesAsync();
                return Ok(newsArticles);
            }

            [Authorize]
            [HttpGet("{id}")]
            public async Task<ActionResult<NewsArticleEnt>> GetArticleById(int id)
            {
                var newsArticle = await _newsService.GetArticleByIdAsync(id);
                var newsArticleDto = NewsMapper<Mapper>.Mapper.Map<NewsArticleDto>(newsArticle);
                if (newsArticleDto == null) return NotFound();
                    return Ok(newsArticleDto);
            }

            [Authorize(Policy = "AdminOnly")]
            [HttpPost("register")]
            public async Task<ActionResult<NewsArticleEnt>>AddArticle([FromBody] NewsArticleDto newsArticleDto)
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

