using LEDLuxe.Core.Interfaces;
using LEDLuxe.Core.Interfaces.Repositories;
using LEDLuxe.Infrastructure.Repositories;
using LEDLuxe.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LEDLuxe.Infrastructure;

public static class DependencyInjectionConfig
{
    public static void AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<IUserRepository, UserRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IApplicationTransaction, ApplicationTransaction>();
    }
}