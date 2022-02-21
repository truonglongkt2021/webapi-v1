using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.Extensions.DependencyInjection;
using StreamFile.Service.Hub;
using System;

namespace StreamFile.WebApi.Extensions
{
    public static class SignalRExtensions
    {
        public static IServiceCollection AddSignalRService(this IServiceCollection services)
        {
            services.AddSignalR()
                .AddHubOptions<DocHub>(options => 
                {
                    options.EnableDetailedErrors = true;
                });

            return services;
        }

        public static IApplicationBuilder UseSignalRService(this IApplicationBuilder app)
        {
            app.UseEndpoints(routes =>
            {
                routes.MapHub<DocHub>("/hub/signalr");
            });
            app.UseCors(builder =>
            {
                builder.WithOrigins("https://localhost:44336",
                                    "https://localhost:44347")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
            return app;
        }
    }
}
