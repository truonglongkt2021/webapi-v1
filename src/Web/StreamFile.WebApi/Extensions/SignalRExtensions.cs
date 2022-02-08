using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StreamFile.Service.Hub;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        //public static IApplicationBuilder UseSignalRService(this IApplicationBuilder app)
        //{   
        //    //app.UseSignalR(routes =>
        //    //{
        //    //    routes.MapHub<UserHub>("/hubs/doc");
        //    //});
        //    return app;
        //}
    }
}
