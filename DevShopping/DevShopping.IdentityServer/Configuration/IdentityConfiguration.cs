using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace DevShopping.IdentityServer.Configuration;

public static class IdentityConfiguration
{
    public const string Admin = nameof(Admin);
    public const string Client = nameof(Client);

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("dev_shooping", "DevShopping Server"),
            new(name: "read", "Read data."),
            new(name: "write", "Write data."),
            new(name: "delete", "Delete data."),
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new()
            {
                ClientId = "client",
                ClientSecrets = { new Secret("my_super_secret".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"read", "write", "profile"}
            },
            new()
            {
                ClientId = "dev_shooping",
                ClientSecrets = { new Secret("my_super_secret".Sha256())},
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = {"https://localhost:4430/signin-oidc"},
                PostLogoutRedirectUris = {"https://localhost:4430/signout-callback-oidc"},
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile,
                    IdentityServerConstants.StandardScopes.Email,
                    "dev_shooping"
                }  
            }
        };

} 
