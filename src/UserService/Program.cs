using Microsoft.IdentityModel.Tokens;
using NewsApp.Infrastructure.Seeding;
using UserService.UserService.API.Auth;
using UserService.UserService.Application.Interfaces;
using UserService.UserService.Application.Services;
using UserService.UserService.Infrastructure.Data;
using UserService.UserService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentityServer()
    .AddInMemoryClients(Config.Clients)
    .AddInMemoryApiScopes(Config.ApiScopes)
    .AddInMemoryIdentityResources(Config.IdentityResources)
    .AddDeveloperSigningCredential()
    .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
    .AddProfileService<ProfileService>();

builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = builder.Configuration["IdentityServer:Url"];
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
});

builder.Services.AddControllers();

// Register your services
builder.Services.AddScoped<DapperContext>(provider =>
    new DapperContext(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserServ>();
builder.Services.AddHostedService<SeedDatabase>();  // Make sure to add the seeding service

var app = builder.Build();

// Ensure that the seeding logic happens during startup.
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<DapperContext>();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();

app.UseIdentityServer();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();