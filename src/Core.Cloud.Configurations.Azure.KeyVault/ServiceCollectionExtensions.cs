using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Cloud.Configurations.Azure.KeyVault
{
    /// <summary>
    /// Extension methods for Azure key vault service collection
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Add <see cref="AzureKeyVault"/> option from section <see cref="Constants.AzureKeyVaultSection"/>
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddAzureKeyVaultOptions(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            return services
                .AddAzureKeyVaultOptions<AzureKeyVault>(configuration, Constants.AzureKeyVaultSection);
        }

        /// <summary>
        /// Add <see cref="AzureKeyVault"/> option from sectionName
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <param name="sectionName">configuration section name</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddAzureKeyVaultOptions(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName)
        {
            return services
                .AddAzureKeyVaultOptions<AzureKeyVault>(configuration, sectionName);
        }

        /// <summary>
        /// Configure <see href="T"/> option from sectionName
        /// </summary>
        /// <typeparam name="T">class</typeparam>
        /// <param name="services"><see cref="IServiceCollection"/></param>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <param name="sectionName">configuration section name</param>
        /// <returns><see cref="IServiceCollection"/></returns>
        public static IServiceCollection AddAzureKeyVaultOptions<T>(
            this IServiceCollection services,
            IConfiguration configuration,
            string sectionName)
            where T : class
        {
            return services.Configure<T>(configuration.AzureKeyVaultConfiguration(sectionName));
        }
    }
}
