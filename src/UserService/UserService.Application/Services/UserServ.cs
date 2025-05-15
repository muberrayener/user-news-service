using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using UserService.UserService.Application.Interfaces;
using UserService.UserService.Core.DTOs;
using UserService.UserService.Core.Entities;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text.Json;
using UserService.UserService.Application.Helpers;

namespace UserService.UserService.Application.Services
{
    public class UserServ : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration; 

        public UserServ(IUserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration; 
        }

        public async Task<IEnumerable<UserEnt>> GetAllUsersAsync() => await _userRepository.GetAllAsync();

        public async Task<UserEnt> GetUserByIdAsync(int id) => await _userRepository.GetByIdAsync(id);

        public async Task<UserEnt> RegisterUserAsync(UserRegistrationDto userRegistrationDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(userRegistrationDto.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists with this email.");
            }

            var user = new UserEnt
            {
                name = userRegistrationDto.Name,
                email = userRegistrationDto.Email,
                password = HashHelpers.HashPassword(userRegistrationDto.Password),
                role = ""
            };

            await _userRepository.AddAsync(user);

            return user;
        }

        public async Task<string> LoginUserAsync(UserLoginDto userLoginDto) 
        {
            var user = await _userRepository.GetByEmailAsync(userLoginDto.Email);
            if (user == null)
            {
                throw new Exception("User doesn't exist");
            }
            if (!HashHelpers.VerifyPassword(userLoginDto.Password, user.password))
            {
                throw new Exception("Wrong password");
            }

            return GenerateJwtToken(user);
        }

        public string GenerateJwtToken(UserEnt user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            if (key.Length < 16)
            {
                throw new ArgumentException("The key must be at least 16 bytes (128 bits) long.");
            }
           
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.role) 
            };
            
           var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
           
           var token = new JwtSecurityToken(
               issuer: _configuration["Jwt:Issuer"],
               audience: _configuration["Jwt:Audience"],
               claims: claims,
               expires: DateTime.Now.AddMinutes(30),
               signingCredentials: creds);
            
             string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
             
            return tokenString;
        }
    }
}