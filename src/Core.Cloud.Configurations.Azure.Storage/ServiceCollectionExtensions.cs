using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Cloud.Configurations.Azure.Storage
{
    /// <summary>
    /// Provides extension methods for adding Azure storage configuration options to the application's service collection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Configures options for <see cref="AzureStorage"/> by binding the configuration section defined by <see cref="Constants.AzureStorageSection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the configuration to.</param>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns></returns>
        public static IServiceCollection AddAzureStorageOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAzureStorageOptions<AzureStorage>(configuration);
            return services;
        }

        /// <summary>
        /// Configures options for a custom options type by binding the configuration section defined by <see cref="Constants.AzureStorageSection"/>.
        /// </summary>
        /// <typeparam name="T">The type of the options class. This type should be a reference type.</typeparam>
        /// <param name="services">The <see cref="IServiceCollection"/> to add the configuration to.</param>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
        public static IServiceCollection AddAzureStorageOptions<T>(this IServiceCollection services, IConfiguration configuration)
            where T : class
        {
            services.Configure<T>(configuration.GetSection(Constants.AzureStorageSection));
            return services;
        }
    }
}
