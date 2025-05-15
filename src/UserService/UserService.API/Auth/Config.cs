using Duende.IdentityServer.Models;

namespace UserService.UserService.API.Auth
{
    public static class Config
    {
        public static IEnumerable<Client> Clients =>
            new[]
            {
            new Client
            {
                ClientId = "client",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedScopes = { "userapi" }
            }
            };

        public static IEnumerable<ApiScope> ApiScopes =>
            new[] { new ApiScope("userapi", "User API") };

        public static IEnumerable<IdentityResource> IdentityResources =>
            new IdentityResource[]
            {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
            };
    }
}
