using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DevShopping.IdentityServer.Model.Context;

public class SQLServerContext : IdentityDbContext<ApplicationUser>
{
    public SQLServerContext(DbContextOptions<SQLServerContext> options) : base(options) { }
}
