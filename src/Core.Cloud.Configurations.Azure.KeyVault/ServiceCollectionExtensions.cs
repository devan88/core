using Core.Cloud.Configurations.Azure.KeyVault;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureKeyVaultOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AzureKeyVault>(configuration.GetSection(Constants.AzureKeyVaultSection));
            return services;
        }

        public static IServiceCollection AddAzureKeyVaultOptions<T>(this IServiceCollection services, IConfiguration configuration)
            where T : class
        {
            services.Configure<T>(configuration.GetSection(Constants.AzureKeyVaultSection));
            return services;
        }
    }
}
