using AlzaShop.Core.Database.Entities;

namespace AlzaShop.Core.Database;

public static class DbInitializer
{
    public static void Seed(AlzaShopDbContext context)
    {
        if (!context.Product.Any())
        {
            context.Product.AddRange(
                new Entities.Product { Name = "Mobile phone", ImgUri = "https://shop.alza.cz/images/mobil.png", Price = 25000, Description = "This is mobile phone description" },
                new Entities.Product { Name = "Laptop", ImgUri = "https://shop.alza.cz/images/laptop.png", Price = 65000, Description = "This is laptop description" },
                new Entities.Product { Name = "Washing maschine", ImgUri = "https://shop.alza.cz/images/washing.png", Price = 15000 },
                new Entities.Product { Name = "Monitor", ImgUri = "https://shop.alza.cz/images/monitor.png", Price = 8000, Description = "This is monitor description" },
                new Entities.Product { Name = "Headphones", ImgUri = "https://shop.alza.cz/images/headphones.png", Price = 2000, Description = "This is headphones description" }
            );
            context.SaveChanges();
        }
    }
}