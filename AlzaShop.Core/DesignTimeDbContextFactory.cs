using AlzaShop.Core.Database;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;

namespace AlzaShop.Core;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AlzaShopDbContext>
{
    public AlzaShopDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AlzaShopDbContext>();
        optionsBuilder.UseSqlServer("Server=(local);Database=AlzaShop;Trusted_Connection=True;TrustServerCertificate=True;");

        return new AlzaShopDbContext(optionsBuilder.Options);
    }
}