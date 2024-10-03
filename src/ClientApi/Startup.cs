using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ClientApi.Application.Services;
using ClientApi.Application.Interfaces;
using ClientApi.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;

namespace ClientApi
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection");

            services.AddScoped<IUserSessionRepository>(provider =>
                new UserSessionRepository(connectionString));

            services.AddAuthentication("Bearer")
            .AddCookie("Bearer", options =>
            {
                options.LoginPath = "/Account/Login"; 
                options.LogoutPath = "/Account/Logout"; 
            });
            services.AddHttpContextAccessor();

            services.AddHttpClient<IUserService, UserService>();
            services.AddHttpClient<INewsService, NewsService>();
            services.AddHttpClient();
            services.AddDistributedMemoryCache(); 
           

            services.AddControllers();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Client API", Version = "v1" });
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseRouting();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Client API V1");
                c.RoutePrefix = "swagger"; 
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); 
            });
        }
    }
}