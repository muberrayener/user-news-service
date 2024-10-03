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

        public async Task<IEnumerable<NewsArticle>> GetAllAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                const string sql = "SELECT * FROM NewsArticles";
                return await connection.QueryAsync<NewsArticle>(sql);
            }
        }

        public async Task<NewsArticle> GetByIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                const string sql = "SELECT * FROM NewsArticles WHERE Id = @Id";
                return await connection.QuerySingleOrDefaultAsync<NewsArticle>(sql, new { Id = id });
            }
        }

        public async Task AddAsync(NewsArticle newsArticle)
        {
            using (var connection = _context.CreateConnection())
            {
                const string sql = "INSERT INTO NewsArticles (Title, Author,Content, Date) VALUES (@Title, @Author, @Content, @Date)";
                await connection.ExecuteAsync(sql, newsArticle);
            }
        }

    }
}
