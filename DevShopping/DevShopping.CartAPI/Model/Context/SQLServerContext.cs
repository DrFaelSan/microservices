using Microsoft.EntityFrameworkCore;

namespace DevShopping.CartAPI.Model.Context;

public class SQLServerContext : DbContext
{
    public SQLServerContext() { }
    public SQLServerContext(DbContextOptions<SQLServerContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<CartDetail> CartDetails { get; set; } = default!;
    public DbSet<CartHeader> CartHeaders { get; set; } = default!;
}
