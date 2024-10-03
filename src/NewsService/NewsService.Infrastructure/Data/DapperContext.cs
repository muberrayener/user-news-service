using Dapper;
using Npgsql;
using System;
using System.Data;

namespace NewsService.NewsService.Infrastructure.Data
{
    public class DapperContext
    {
        private readonly string _connectionString;

        public DapperContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection CreateConnection() => new NpgsqlConnection(_connectionString);

        public void EnsureDatabaseCreated()
        {
            using (var connection = CreateConnection())
            {
                try
                {
                    connection.Open();

                    const string sql = @"
                        CREATE TABLE IF NOT EXISTS NewsArticles (
                            id SERIAL PRIMARY KEY,
                            title VARCHAR(255) NOT NULL,
                            content TEXT NOT NULL,
                            author VARCHAR(100) NOT NULL,
                            date VARCHAR(100)
                        );";

                    connection.Execute(sql);
                    Console.WriteLine("Table checked/created successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred while creating the database: {ex.Message}");
                }
            }
        }
    }
}