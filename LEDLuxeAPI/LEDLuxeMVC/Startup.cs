using LEDLuxe.Application;
using LEDLuxe.Core;
using LEDLuxe.Infrastructure;
using LEDLuxe.Infrastructure.Bootstrap;

namespace LEDLuxe.Web;

public class Startup(IConfiguration configuration)
{
    public IConfiguration Configuration { get; } = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddDatabaseConfiguration(Configuration);

        services.AddControllersWithViews();

        services.AddCoreLayer();
        services.AddApplicationLayer();
        services.AddInfrastructureLayer();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        else
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        });
    }
}