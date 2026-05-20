using BusinessLogicLayer.Mappers;
using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataBusinessLogicLayer(this IServiceCollection services)
    {
        //Todo :Add DataAccessLayer into the IOC container
        services.AddAutoMapper(cfg => { }, typeof(ProductAddRequestToProductMappingProfile));
        return services;
    }
}