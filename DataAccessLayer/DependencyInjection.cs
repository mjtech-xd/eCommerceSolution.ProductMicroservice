using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataAccessLayer(this IServiceCollection services)
    {
        //Todo :Add DataAccessLayer into the IOC container
        return services;
    }
}