using DevShooping.IdentityServer.Configuration;
using DevShooping.IdentityServer.Model;
using DevShooping.IdentityServer.Model.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace DevShooping.IdentityServer.Initializer;

public class DbInitializer : IDbInitializer
{
    private readonly SQLServerContext _context;
    private readonly UserManager<ApplicationUser> _user;
    private readonly RoleManager<IdentityRole> _role;

    public DbInitializer(
        SQLServerContext SQLServerContext, 
        UserManager<ApplicationUser> user, 
        RoleManager<IdentityRole> role
    )
    {
        _context = SQLServerContext;
        _user = user;
        _role = role;
    }

    public void Initialize()
    {
        if (_role.FindByNameAsync(IdentityConfiguration.Admin).Result is not null) return;
        _role.CreateAsync(new IdentityRole(IdentityConfiguration.Admin)).GetAwaiter().GetResult();
        _role?.CreateAsync(new IdentityRole(IdentityConfiguration.Client)).GetAwaiter().GetResult();

        #region Admin
        ApplicationUser admin = new()
        {
            UserName = "Rafael-admin",
            Email = "rafael-admin@email.com.br",
            EmailConfirmed = true,
            PhoneNumber = "+55 (11) 90000-0000",
            FirstName = "Rafael",
            LastName = "Admin"
        };

        _user.CreateAsync(admin, "RafaAdmin123$")
             .GetAwaiter()
             .GetResult();

        _user.AddToRoleAsync(admin, IdentityConfiguration.Admin)
             .GetAwaiter()
             .GetResult();
        _ = _user.AddClaimsAsync(admin, new Claim[]
                               {
                                   new(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                                   new(JwtClaimTypes.GivenName, admin.FirstName),
                                   new(JwtClaimTypes.FamilyName, admin.LastName),
                                   new(JwtClaimTypes.Role, IdentityConfiguration.Admin)
                               }).Result;
        #endregion

        #region Client
        ApplicationUser client = new()
        {
            UserName = "Rafael-client",
            Email = "rafael-client@email.com.br",
            EmailConfirmed = true,
            PhoneNumber = "+55 (11) 90000-0000",
            FirstName = "Rafael",
            LastName = "Client"
        };

        _user.CreateAsync(client, "Rafa123$")
             .GetAwaiter()
             .GetResult();

        _user.AddToRoleAsync(client, IdentityConfiguration.Client)
             .GetAwaiter()
             .GetResult();
        _ = _user?.AddClaimsAsync(client, new Claim[]
                               {
                                   new(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                                   new(JwtClaimTypes.GivenName, client.FirstName),
                                   new(JwtClaimTypes.FamilyName, client.LastName),
                                   new(JwtClaimTypes.Role, IdentityConfiguration.Client)
                               }).Result;
        #endregion

    }
}
