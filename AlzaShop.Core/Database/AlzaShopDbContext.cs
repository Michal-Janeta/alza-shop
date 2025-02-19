using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace AlzaShop.Core.Database;

public class AlzaShopDbContext : DbContext
{
    public AlzaShopDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Entities.Product> Product { get; set; }

    protected override void OnModelCreating(ModelBuilder model)
    {

    }
}
