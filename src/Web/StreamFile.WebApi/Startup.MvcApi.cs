using StreamFile.WebApi.Filters.Validation;
using Microsoft.Extensions.DependencyInjection;

namespace StreamFile.WebApi
{
    public static class StartupMvcApi
    {
        #region Services

        public static IServiceCollection AddMvcApi(this IServiceCollection services)
        {
            // Validation Filters

            services.AddScoped<ApiValidationActionFilterAttribute>();
            return services;
        }

        #endregion
    }
}