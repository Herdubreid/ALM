using Blazor.Extensions.Storage;
using Blazored.Localisation;
using Blazored.Toast;
using BlazorState;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using Toolbelt.Blazor.Extensions.DependencyInjection;

namespace Celin
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddBlazoredLocalisation();
            services.AddBlazoredToast();
            services.AddStorage();
            services.AddBlazorState(
                (options) =>
                {
                    options.UseCloneStateBehavior = false;
#if DEBUG
                    options.UseReduxDevToolsBehavior = true;
#endif
                    options.Assemblies = new Assembly[]
                    {
                        typeof(Startup).GetTypeInfo().Assembly
                    };
                });
            services.AddScoped<AppState>();
            services.AddScoped<LocationService>();
        }

        public void Configure(IComponentsApplicationBuilder app)
        {
            app.UseBlazoredLocalisation();
            app.UseLocalTimeZone();
            app.AddComponent<App>("app");
        }
    }
}
