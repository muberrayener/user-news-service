using Dapper;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Data;
using UserService.UserService.Core.Entities;

namespace UserService.UserService.Infrastructure.Data
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

                try
                {
                    connection.Open();

                    const string sql = @"
                    CREATE TABLE IF NOT EXISTS users (
                        id SERIAL PRIMARY KEY,
                        name VARCHAR(100) NOT NULL,
                        email VARCHAR(100) NOT NULL UNIQUE,
                        password VARCHAR(100) NOT NULL,
                        role VARCHAR(100)
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
