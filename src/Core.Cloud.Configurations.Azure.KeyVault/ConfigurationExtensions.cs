using Microsoft.Extensions.Configuration;

namespace Core.Cloud.Configurations.Azure.KeyVault
{
    /// <summary>
    /// Extension methods for Azure key vault configurations.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Get azure key vault configuration from section in <see cref="Constants.AzureKeyVaultsSection"/>
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns>a list of <see cref="AzureKeyVault"/></returns>
        public static IEnumerable<AzureKeyVault> GetAzureKeyVaults(this IConfiguration configuration)
        {
            return configuration.GetAzureKeyVaults(Constants.AzureKeyVaultsSection);
        }

        /// <summary>
        /// Get azure key vault configuration from sectionName
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <param name="sectionName">configuration section name</param>
        /// <returns>a list of <see cref="AzureKeyVault"/></returns>
        public static IEnumerable<AzureKeyVault> GetAzureKeyVaults(
            this IConfiguration configuration,
            string sectionName)
        {
            return configuration
                .AzureKeyVaultConfiguration(sectionName)
                .Get<IEnumerable<AzureKeyVault>>() ?? [];
        }

        /// <summary>
        /// Bind azure key vault configuration
        /// from section in <see cref="Constants.AzureKeyVaultSection"/> to class T
        /// </summary>
        /// <typeparam name="T">class with parameterless constructor</typeparam>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns>a new <see href="T"/> class binded with configuration</returns>
        public static T GetAzureKeyVault<T>(this IConfiguration configuration)
            where T : new()
        {
            return configuration.GetAzureKeyVault<T>(Constants.AzureKeyVaultSection);
        }

        /// <summary>
        /// Bind azure key vault configuration from sectionName to class T
        /// </summary>
        /// <typeparam name="T">class with parameterless constructor</typeparam>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <param name="sectionName">configuration section name</param>
        /// <returns>a new <see href="T"/> class binded with configuration</returns>
        public static T GetAzureKeyVault<T>(
            this IConfiguration configuration,
            string sectionName)
            where T : new()
        {
            return configuration
                .AzureKeyVaultConfiguration(sectionName)
                .Get<T>() ?? new T();
        }

        /// <summary>
        /// Get azure key vault section in <see cref="Constants.AzureKeyVaultSection"/>
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <returns><see cref="IConfigurationSection"/></returns>
        public static IConfigurationSection AzureKeyVaultConfiguration(this IConfiguration configuration)
        {
            return configuration.AzureKeyVaultConfiguration(Constants.AzureKeyVaultSection);
        }

        /// <summary>
        /// Get azure key vault section from sectionName
        /// </summary>
        /// <param name="configuration"><see cref="IConfiguration"/></param>
        /// <param name="sectionName">configuration section name</param>
        /// <returns><see cref="IConfigurationSection"/></returns>
        public static IConfigurationSection AzureKeyVaultConfiguration(
            this IConfiguration configuration,
            string sectionName)
        {
            return configuration.GetSection(sectionName);
        }
    }
}
