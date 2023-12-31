using Azure.Core;
using Core.Azure.Identity;

namespace Core.Cloud.Configurations.Azure.Storage
{
    public class AzureStorage : ClientOptions
    {
        public required string Name { get; set; }
        public Uri Uri() => new($"https://{Name}.blob.core.windows.net/");
        public IEnumerable<ServicePrincipalCredential> ServicePrincipalCredentials { get; set; } = new List<ServicePrincipalCredential>();
    }
}
