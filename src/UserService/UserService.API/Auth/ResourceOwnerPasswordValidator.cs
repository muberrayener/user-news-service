using Duende.IdentityServer.Validation;
using Duende.IdentityServer.Models;
using System.Threading.Tasks;
using UserService.UserService.Application.Interfaces;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Cryptography;
using System.Text;
using UserService.UserService.Application.Helpers;

namespace UserService.UserService.API.Auth
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository _userRepository;

        public ResourceOwnerPasswordValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userRepository.GetByEmailAsync(context.UserName);
            if (user == null || !HashHelpers.VerifyPassword(context.Password, user.password))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid credentials");
                return;
            }

            context.Result = new GrantValidationResult(
                subject: user.id.ToString(),
                authenticationMethod: GrantType.ResourceOwnerPassword,
                claims: new[]
                {
                new Claim("name", user.name),
                new Claim("email", user.email),
                new Claim("role", user.role)
                });
        }

        
    }

}
