using AutoMapper;
using DevShooping.ProductAPI.Data.ValueObjects;
using DevShooping.ProductAPI.Model;

namespace DevShooping.ProductAPI.Extensions;

public static class MappingExtensions
{
    public static MapperConfiguration Maps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<ProductVO, Product>();
            config.CreateMap<Product, ProductVO>();
        });

        return mappingConfig;
    }

    public static void AddMappingCongifs(this IServiceCollection services)
    {
        IMapper mapper = Maps().CreateMapper();
        services.AddSingleton(mapper);
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}
