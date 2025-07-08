using Microsoft.Extensions.Configuration;

namespace Core.HttpClient.Extensions
{
    /// <summary>
    /// Provides extension methods for the http client configurations.
    /// </summary>
    public static class ConfigurationExtension
    {
        /// <summary>
        /// Retrieves the configuration section for the Http client settings.
        /// </summary>
        /// <param name="configuration">Retrieves configuration from the constant section
        /// name defined in <seealso cref="Constant.HttpClientSection"/>.</param>
        /// <returns>The configuration section corresponding to the specified section name.</returns>
        public static IConfiguration GetHttpClient(this IConfiguration configuration)
            => configuration.GetSection(Constant.HttpClientSection);
    }
}
