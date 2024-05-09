using DevShopping.CartAPI.Repository;
using DevShopping.CartAPI.Repository.Interfaces;

namespace DevShopping.CartAPI.Ioc;

public static class InjectionDependecy
{
    public static void RegisterRepositorys(this IServiceCollection services)
    {
        services.AddScoped<ICartRepository, CartRepository>();
    }
}
