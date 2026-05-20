using Microsoft.Extensions.DependencyInjection;

namespace BusinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataBusinessLogicLayer(this IServiceCollection services)
    {
        //Todo :Add DataAccessLayer into the IOC container
        return services;
    }
}