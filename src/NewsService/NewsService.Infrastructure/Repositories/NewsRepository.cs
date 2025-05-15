using Dapper;
using NewsService.NewsService.Core.Entities;
using NewsService.NewsService.Core.Interfaces;
using NewsService.NewsService.Infrastructure.Data;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;


namespace NewsService.NewsService.Infrastructure.Repositories
{
    public class NewsRepository : INewsRepository
    {

        private readonly DapperContext _context;

        public NewsRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NewsArticleEnt>> GetAllAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                const string sql = "SELECT * FROM NewsArticles";
                return await connection.QueryAsync<NewsArticleEnt>(sql);
            }
        }

        public async Task<NewsArticleEnt> GetByIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                const string sql = "SELECT * FROM NewsArticles WHERE Id = @Id";
                return await connection.QuerySingleOrDefaultAsync<NewsArticleEnt>(sql, new { Id = id });
            }
        }

        public async Task AddAsync(NewsArticleEnt newsArticle)
        {
            using (var connection = _context.CreateConnection())
            {
                const string sql = "INSERT INTO NewsArticles (Title, Author,Content, insert_date) VALUES (@Title, @Author, @Content, @insert_date)";
                await connection.ExecuteAsync(sql, newsArticle);
            }
        }

    }
}
