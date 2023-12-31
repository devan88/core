using Core.Cloud.Configurations.Azure.Storage;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureStorageOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureStorage>(configuration.GetSection(Constants.AzureStorageSection));
            return services;
        }

        public static IServiceCollection AddAzureStorageOptions<T>(this IServiceCollection services, IConfiguration configuration)
            where T : class
        {
            services.Configure<T>(configuration.GetSection(Constants.AzureStorageSection));
            return services;
        }
    }
}
