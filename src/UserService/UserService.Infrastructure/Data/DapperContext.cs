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

        

    }

}
