using Core.Cloud.Configurations.Azure.Storage;

namespace Microsoft.Extensions.Configuration
{
    public static class ConfigurationExtensions
    {

        public static IEnumerable<AzureStorage> GetAzureStorages(this IConfiguration configuration)
        {
            return configuration.AzureStorageConfiguration().Get<IEnumerable<AzureStorage>>() ?? new List<AzureStorage>();
        }

        public static T GetAzureStorage<T>(this IConfiguration configuration)
            where T : new()
        {
            return configuration.AzureStorageConfiguration().Get<T>() ?? new T();
        }

        public static IConfigurationSection AzureStorageConfiguration(this IConfiguration configuration)
        {
            return configuration.GetSection(Constants.AzureStorageSection);
        }
    }
}
