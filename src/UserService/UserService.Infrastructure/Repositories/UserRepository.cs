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

        public async Task<IEnumerable<UserEnt>> GetAllAsync()
        {
            using (var connection = _context.CreateConnection())
            {
                const string sql = "SELECT * FROM Users";
                return await connection.QueryAsync<UserEnt>(sql);
            }
        }

        public async Task<UserEnt> GetByIdAsync(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                const string sql = "SELECT * FROM Users WHERE Id = @Id";
                return await connection.QuerySingleOrDefaultAsync<UserEnt>(sql, new { Id = id });
            }
        }

        public async Task AddAsync(UserEnt user)
        {
            using (var connection = _context.CreateConnection())
            {
                const string sql = "INSERT INTO Users (Name, Email,Password, Role) VALUES (@Name, @Email, @Password, @Role)";
                await connection.ExecuteAsync(sql, user);
            }
        }

        public async Task<UserEnt> GetByEmailAsync(string email)
        {
            const string sql = "SELECT * FROM users WHERE email = @Email;";

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<UserEnt>(sql, new { Email = email });
            }
        }

    }
}