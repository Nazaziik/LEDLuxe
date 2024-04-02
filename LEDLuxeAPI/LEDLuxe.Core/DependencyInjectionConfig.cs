using LEDLuxe.Core.Interfaces.Services;
using LEDLuxe.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LEDLuxe.Core;

public static class DependencyInjectionConfig
{
    public static void AddCoreLayer(this IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IProductService, ProductService>();
    }
}