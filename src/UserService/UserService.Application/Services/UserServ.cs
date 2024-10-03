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

        public async Task<IEnumerable<User>> GetAllUsersAsync() => await _userRepository.GetAllAsync();

        public async Task<User> GetUserByIdAsync(int id) => await _userRepository.GetByIdAsync(id);

        public async Task<User> RegisterUserAsync(UserRegistrationDto userRegistrationDto)
        {
            var existingUser = await _userRepository.GetByEmailAsync(userRegistrationDto.Email);
            if (existingUser != null)
            {
                throw new Exception("User already exists with this email.");
            }

            var user = new User
            {
                Name = userRegistrationDto.Name,
                Email = userRegistrationDto.Email,
                Password = HashPassword(userRegistrationDto.Password),
                Role = ""
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
            if (!VerifyPassword(userLoginDto.Password, user.Password))
            {
                throw new Exception("Wrong password");
            }

            return GenerateJwtToken(user);
        }

        private const string Key = "secret_key";

        private string HashPassword(string password)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Key));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(Key));
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            var hashBytes = Convert.FromBase64String(storedHash);
            return computedHash.SequenceEqual(hashBytes);
        }

        public async Task EnsureAdminUserExistsAsync()
        {
            var adminEmail = "admin@example.com";
            var adminPassword = "123123";
            var adminUser = await _userRepository.GetByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var hashedPassword = HashPassword(adminPassword);
                adminUser = new User
                {
                    Name = "ADMIN",
                    Email = adminEmail,
                    Password = hashedPassword,
                    Role = "admin"
                };

                await _userRepository.AddAsync(adminUser);
                Console.WriteLine("Admin created successfully.");
            }
        }

        public string GenerateJwtToken(User user)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);
            if (key.Length < 16)
            {
                throw new ArgumentException("The key must be at least 16 bytes (128 bits) long.");
            }
           
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role) 
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