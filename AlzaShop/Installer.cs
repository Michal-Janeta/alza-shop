using AlzaShop.Core.Database;

namespace AlzaShop.Api;

public class Installer
{
    public static void Register(IServiceCollection services)
    {
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    }
}
