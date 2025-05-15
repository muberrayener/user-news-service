using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using System.Security.Claims;
using UserService.UserService.Application.Interfaces;

namespace UserService.UserService.Application.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository _userRepository;

        public ProfileService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            // Extract user email or subject (email or ID)
            var user = await _userRepository.GetByEmailAsync(context.Subject.GetSubjectId());

            // Add custom claims to the profile response
            var claims = new List<Claim>
        {
            new Claim("name", user.name),
            new Claim("email", user.email),
            new Claim("role", user.role),
            // Add more claims as needed
        };

            context.IssuedClaims = claims;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            // Check if the user is active (can add custom logic if needed)
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}