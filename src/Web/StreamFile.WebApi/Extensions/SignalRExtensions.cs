using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StreamFile.Service.Hub;

namespace StreamFile.WebApi.Extensions
{
    public static class SignalRExtensions
    {
        public static IServiceCollection AddSignalRService(this IServiceCollection services)
        {
            services.AddSignalR()
                .AddHubOptions<DocHub>(options => { options.EnableDetailedErrors = true; });

            return services;
        }

        public static IApplicationBuilder UseSignalRService(this IApplicationBuilder app)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<DocHub>("/hub/link");
            });
            return app;
        }
    }
}
