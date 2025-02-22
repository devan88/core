using Microsoft.Extensions.Configuration;

namespace Core.HttpClient.Authorization
{
    /// <summary>
    /// A factory for creating instances of <see cref="IOAuthTokenProviderService"/>.
    /// </summary>
    public interface IOAuthTokenProviderFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="IOAuthTokenProviderService"/> based on the provided configuration
        /// and cloud provider.
        /// </summary>
        /// <param name="configuration">The configuration settings used to create the token provider.</param>
        /// <param name="cloudProvider">The cloud provider for which the token provider will be created.</param>
        /// <returns>An instance of <see cref="IOAuthTokenProviderService"/> configured for the specified cloud provider.</returns>
        IOAuthTokenProviderService CreateOAuthTokenProviderService(IConfiguration configuration, CloudProvider cloudProvider);
    }
}
