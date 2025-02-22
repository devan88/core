using Azure.Core;

namespace Core.Cloud.Configurations.Azure.KeyVault
{
    /// <summary>
    /// Azure key vault options
    /// </summary>
    public class AzureKeyVault : ClientOptions
    {
        /// <summary>
        /// The key vault name
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The uri of the key vault
        /// </summary>
        /// <returns>https://{Name}.vault.azure.net/</returns>
        public Uri Uri() => new($"https://{Name}.vault.azure.net/");
    }
}
