using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Configuration;

namespace Core.Cloud.Configurations.Azure.Storage
{
    /// <summary>
    /// Extension methods for Azure storage configurations
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Get azure storage configuration from section in <see cref="Constants.AzureStoragesSection"/>
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns>a list of <see cref="AzureStorage"/></returns>
        public static IEnumerable<AzureStorage> GetAzureStorages(this IConfiguration configuration)
        {
            return configuration.GetAzureStorage<IEnumerable<AzureStorage>>(Constants.AzureStoragesSection) ?? [];
        }

        /// <summary>
        /// Get azure storage configuration from section in <see cref="Constants.AzureStorageSection"/>
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns><see cref="AzureStorage"/></returns>
        public static AzureStorage? GetAzureStorage(this IConfiguration configuration)
        {
            return configuration.GetAzureStorage<AzureStorage>(Constants.AzureStorageSection);
        }

        /// <summary>
        /// Bind azure storage configuration from sectionName to class T.
        /// </summary>
        /// <typeparam name="T">The type of the new instance to bind.</typeparam>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <param name="sectionName">configuration section name.</param>
        /// <returns>The new instance of T if successful, default(T) otherwise.</returns>
        public static T? GetAzureStorage<T>(
            this IConfiguration configuration,
            string sectionName)
        {
            return configuration.AzureStorageConfiguration(sectionName).Get<T>();
        }

        /// <summary>
        /// Get azure storage configuration from section in <see cref="Constants.AzureStorageSection"/>.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns><see cref="IConfigurationSection"/></returns>
        public static IConfigurationSection AzureStorageConfiguration(
            this IConfiguration configuration)
        {
            return configuration.GetSection(Constants.AzureStorageSection);
        }

        /// <summary>
        /// Get azure storage configuration section from sectionName.
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <param name="sectionName">configuration section name.</param>
        /// <returns><see cref="IConfigurationSection"/></returns>
        public static IConfigurationSection AzureStorageConfiguration(
            this IConfiguration configuration,
            string sectionName)
        {
            return configuration.GetSection(sectionName);
        }
    }
}
