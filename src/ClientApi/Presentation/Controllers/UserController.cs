using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using ClientApi.Application.Interfaces;
using ClientApi.Core.DTOs;
using Microsoft.AspNetCore.Session;
using ClientApi.Core.Entities;
using ClientApi.Infrastructure.Repositories;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ClientApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserSessionRepository _userSessionRepository;
        public UserController(IUserService userService, IUserSessionRepository userSessionRepository)
        {
            _userService = userService;
            _userSessionRepository = userSessionRepository;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromBody] RegisterRequest request)
        {
            await _userService.RegisterAsync(request);
            return CreatedAtAction(nameof(Register), new { name = request.Name }, request);
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest request)
        {
            var token = await _userService.LoginAsync(request);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            var userSession = new UserSession
            {
                Email = request.Email,
                Token = token
            };

            await _userSessionRepository.AddSessionAsync(userSession);

            var claims = new[] { new Claim(ClaimTypes.Name, request.Email) };
            var claimsIdentity = new ClaimsIdentity(claims, "Bearer");

            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

            await HttpContext.SignInAsync("Bearer", claimsPrincipal);

            return Ok(new { Message = "Login successful.", Token = token });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
        {
            var email = User.Identity.Name;
            var token = await _userSessionRepository.GetTokenAsync(email);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is missing.");
            }

            var users = await _userService.GetAllUsersAsync(token);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetUserById(int id)
        {
            var email = User.Identity.Name;
            var token = await _userSessionRepository.GetTokenAsync(email);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized("Token is empty.");
            }
            var user = await _userService.GetUserByIdAsync(id, token);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
    }
}