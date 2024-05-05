using Microsoft.AspNetCore.Identity;

namespace DevShooping.IdentityServer.Model;

public class ApplicationUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}
