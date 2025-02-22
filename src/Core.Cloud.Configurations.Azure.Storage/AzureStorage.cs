using Azure.Core;
using Core.Cloud.Azure.Identity;

namespace Core.Cloud.Configurations.Azure.Storage
{
    /// <summary>
    /// Azure storage options
    /// </summary>
    public class AzureStorage : ClientOptions
    {
        /// <summary>
        /// The storage name
        /// </summary>
        public required string Name { get; set; }

        /// <summary>
        /// The uri of the storage
        /// </summary>
        /// <returns>https://{Name}.blob.core.windows.net/</returns>
        public Uri Uri() => new($"https://{Name}.blob.core.windows.net/");

        /// <summary>
        /// A list of <see cref="ServicePrincipalCredential"/> for authentication
        /// </summary>
        public IEnumerable<ServicePrincipalCredential> ServicePrincipalCredentials { get; set; } = [];
    }
}
