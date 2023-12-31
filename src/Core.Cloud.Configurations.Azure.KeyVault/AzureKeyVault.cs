using Azure.Core;

namespace Core.Cloud.Configurations.Azure.KeyVault
{
    public class AzureKeyVault : ClientOptions
    {
        public required string Name { get; set; }
        public Uri Uri() => new($"https://{Name}.vault.azure.net/");
    }
}
