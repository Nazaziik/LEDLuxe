using Microsoft.Extensions.DependencyInjection;

namespace LEDLuxe.Application;

public static class DependencyInjectionConfig
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    }
}
