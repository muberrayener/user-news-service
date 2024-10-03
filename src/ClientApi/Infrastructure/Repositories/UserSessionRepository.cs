using Dapper;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using ClientApi.Application.Interfaces;
using ClientApi.Core.Entities;
using Npgsql;
using System.IdentityModel.Tokens.Jwt;

namespace ClientApi.Infrastructure.Repositories
{
    public class UserSessionRepository : IUserSessionRepository
    {
        private readonly string _connectionString;

        public UserSessionRepository(string connectionString)
        {
            _connectionString = connectionString;
            EnsureTableExists().Wait();
        }

        private async Task EnsureTableExists()
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                var sql = @"
                CREATE TABLE IF NOT EXISTS UserSession (
                    Id SERIAL PRIMARY KEY,
                    Email VARCHAR(255) NOT NULL UNIQUE,
                    Token TEXT NOT NULL
                )";

                await db.ExecuteAsync(sql);
            }
        }
        public async Task AddSessionAsync(UserSession session)
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                var sql = @"
                    INSERT INTO UserSession (Email, Token) 
                    VALUES (@Email, @Token)
                    ON CONFLICT (Email) 
                    DO UPDATE SET Token = EXCLUDED.Token";

                await db.ExecuteAsync(sql, session);
            }
        }

        public async Task<UserSession> GetSessionAsync(string email)
        {
            using (IDbConnection db = new NpgsqlConnection(_connectionString))
            {
                var sql = "SELECT * FROM UserSession WHERE Email = @Email";
                return await db.QuerySingleOrDefaultAsync<UserSession>(sql, new { Email = email });
            }
        }

        public async Task<string> GetTokenAsync(string email)
        {
            var userSession = await GetSessionAsync(email);
            return userSession?.Token;
        }

    }
}