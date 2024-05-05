using DevShooping.IdentityServer.Model.Context;
using Microsoft.EntityFrameworkCore;

namespace DevShooping.IdentityServer.Extensions;

public static class ContextsExtensions
{
    public static void AddContexts(this IServiceCollection services, IConfiguration configuration)
    {
        string? connectionString = configuration.GetConnectionString("SQLConnectionString");
        if(string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
        services.AddDbContext<SQLServerContext>(options => options.UseSqlServer(connectionString));
    }
}
