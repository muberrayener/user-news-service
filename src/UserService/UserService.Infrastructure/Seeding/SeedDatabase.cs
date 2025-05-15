
using System.Data;
using System.Data.Common;
using System.Reflection;
using System.Text.Json;
using System.Xml.Linq;
using Dapper;
using Dapper.Contrib.Extensions;
using DbUp;
using Microsoft.AspNetCore.Http.HttpResults;
using Npgsql;
using UserService.UserService.Application.Helpers;
using UserService.UserService.Core.Entities;
using UserService.UserService.Infrastructure.Data;
using static Duende.IdentityServer.Models.IdentityResources;

namespace NewsApp.Infrastructure.Seeding;

public class SeedDatabase : BackgroundService
{
    private readonly IConfiguration configuration;
    private readonly ILogger<SeedDatabase> logger;

    public SeedDatabase(IConfiguration configuration, ILogger<SeedDatabase> logger)
    {
        this.configuration = configuration;
        this.logger = logger;

    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Migration();
        await SeedAdminUserAsync();
    }

    private void Migration()
    {
        logger.LogInformation("Starting database migration...");

        string connectionString = configuration.GetConnectionString("DefaultConnection");
        string executingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

        var upgrader = DeployChanges.To
    .PostgresqlDatabase(connectionString)
    .WithScriptsFromFileSystem(Path.Combine(executingDir, "UserService.Infrastructure", "DBUp"))
    //.WithScript("CreateSchemaIfNotExists", @"
    //    DO $$  
    //    BEGIN  
    //        RAISE NOTICE 'Checking if schema exists...';
    //        IF NOT EXISTS (SELECT 1 FROM information_schema.schemata WHERE schema_name = 'newsapp') THEN  
    //           RAISE NOTICE 'Creating schema newsapp';
    //           EXECUTE 'CREATE SCHEMA ""newsapp""';  
    //        ELSE
    //           RAISE NOTICE 'Schema newsapp already exists';
    //        END IF;  
    //    END $$;
    //")
    .WithTransactionPerScript()
    .WithVariablesDisabled()
    .JournalToPostgresqlTable("public", "schemaversions")
    .LogToConsole()
    .Build();

        if (upgrader.IsUpgradeRequired())
        {
            var result = upgrader.PerformUpgrade();
            if (!result.Successful)
            {
                logger.LogError(result.Error, "Migration failed.");
            }
            else
            {
                logger.LogInformation("Migration successful.");
            }
        }
    }
    
    public async Task SeedAdminUserAsync()
    {
        // Retrieve connection string from configuration
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        // Read the admin seed data from the JSON file
        string executingDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
        var json = await File.ReadAllTextAsync(Path.Combine(executingDir, "UserService.Infrastructure","SeedData/seed.admin.json"));
        var admin = JsonSerializer.Deserialize<UserEnt>(json);

        if (admin == null)
            throw new Exception("Admin seed file is invalid.");

        // Use the NpgsqlConnection to interact with PostgreSQL database
        using (var connection = new NpgsqlConnection(connectionString))
        {
            // Open the database connection
            await connection.OpenAsync();

            // Check if the admin user exists
            var adminUser = (await connection.QueryFirstOrDefaultAsync<UserEnt>(
                    $"select * from {TableName<UserEnt>.Get} WHERE Email = @admin.Email",
                    new {admin.email }));

            // If admin user doesn't exist, create a new one
            if (adminUser == null)
            {
                // Hash the admin's password
                var hashedPassword = HashHelpers.HashPassword(admin.password);
                admin.password = hashedPassword;
                admin.insert_date = DateTime.UtcNow;

                await connection.InsertAsync(admin);

                logger.LogInformation("Admin created successfully.");
            }
            else
            {
                logger.LogInformation("Admin user already exists.");
            }
        }
    }
}