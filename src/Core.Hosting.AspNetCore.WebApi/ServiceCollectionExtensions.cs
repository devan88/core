using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApiControllers(
            this IServiceCollection services,
            Action<MvcOptions>? configure = null)
        {
            services.AddControllers(configure);
            services.AddEndpointsApiExplorer();

            return services;
        }
    }
}
