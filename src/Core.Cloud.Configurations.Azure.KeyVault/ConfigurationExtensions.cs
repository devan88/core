using Core.Cloud.Configurations.Azure.KeyVault;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {

        public static IEnumerable<AzureKeyVault> GetAzureKeyVaults(this IConfiguration configuration)
        {
            return configuration.AzureKeyVaultConfiguration().Get<IEnumerable<AzureKeyVault>>() ?? new List<AzureKeyVault>();
        }

        public static T GetAzureKeyVault<T>(this IConfiguration configuration)
            where T : new()
        {
            return configuration.AzureKeyVaultConfiguration().Get<T>() ?? new T();
        }

        public static IConfigurationSection AzureKeyVaultConfiguration(this IConfiguration configuration)
        {
            return configuration.GetSection(Constants.AzureKeyVaultSection);
        }
    }
}
