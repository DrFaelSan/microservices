﻿using DevShooping.ProductAPI.Repository;

namespace DevShooping.ProductAPI.Ioc;

public static class InjectionDependecy
{
    public static void RegisterRepositorys(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
    }
}
