using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ClientApi.Application.Interfaces;
using ClientApi.Core.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace ClientApi.Application.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;

        public UserService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> LoginAsync(LoginRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5005/users/login", request);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Response Status: {response.StatusCode}, Content: {responseContent}");

            response.EnsureSuccessStatusCode();

            return responseContent;
        }

        public async Task<UserDto> RegisterAsync(RegisterRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("http://localhost:5005/users/register", request);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<UserDto>();
        }

        public async Task<IEnumerable<UserDto>> GetAllUsersAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var users = await _httpClient.GetFromJsonAsync<IEnumerable<UserDto>>("http://localhost:5005/users");

            _httpClient.DefaultRequestHeaders.Authorization = null;
            return users;
        }

        public async Task<UserDto> GetUserByIdAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var user = await _httpClient.GetFromJsonAsync<UserDto>($"http://localhost:5005/users/{id}");
            _httpClient.DefaultRequestHeaders.Authorization = null;

            return user;
        }
    }
}