using AutoMapper;
using DevShopping.CartAPI.Data.ValueObjects;
using DevShopping.CartAPI.Model;

namespace DevShopping.CartAPI.Extensions;

public static class MappingExtensions
{
    public static MapperConfiguration Maps()
    {
        var mappingConfig = new MapperConfiguration(config =>
        {
            config.CreateMap<ProductVO, Product>().ReverseMap();
            config.CreateMap<CartDetailVO, CartDetail>().ReverseMap();
            config.CreateMap<CartHeaderVO, CartHeader>().ReverseMap();
            config.CreateMap<CartVO, Cart>().ReverseMap();
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
