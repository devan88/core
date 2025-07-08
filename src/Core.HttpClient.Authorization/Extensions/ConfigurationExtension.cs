using Microsoft.Extensions.Configuration;

namespace Core.HttpClient.Authorization.Extensions
{
    /// <summary>
    /// Provides extension methods for the http client authorization configurations.
    /// </summary>
    public static class ConfigurationExtension
    {
        /// <summary>
        /// Retrieves the configuration section for the Http client authorization settings.
        /// </summary>
        /// <param name="configuration">Retrieves configuration from the constant section
        /// name defined in <seealso cref="Constant.HttpClientAuthorizationSection"/>.</param>
        /// <returns>The configuration section corresponding to the specified section name.</returns>
        public static IConfiguration GetHttpClientAuthorization(this IConfiguration configuration)
            => configuration.GetSection(Constant.HttpClientAuthorizationSection);
    }
}
