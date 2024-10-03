using Dapper;
using UserService.UserService.Core.Entities;
using UserService.UserService.Infrastructure.Data;
using UserService.UserService.Application.Interfaces;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace UserService.UserService.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DapperContext _context;

        public UserRepository(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                const string sql = "SELECT * FROM Users";
                return await connection.QueryAsync<User>(sql);
            }
        }

        public async Task<User> GetByIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                const string sql = "SELECT * FROM Users WHERE Id = @Id";
                return await connection.QuerySingleOrDefaultAsync<User>(sql, new { Id = id });
            }
        }

        public async Task AddAsync(User user)
        {
            using (var connection = _context.CreateConnection())
            {
                const string sql = "INSERT INTO Users (Name, Email,Password, Role) VALUES (@Name, @Email, @Password, @Role)";
                await connection.ExecuteAsync(sql, user);
            }
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            const string sql = "SELECT * FROM users WHERE email = @Email;";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<User>(sql, new { Email = email });
            }
        }

    }
}