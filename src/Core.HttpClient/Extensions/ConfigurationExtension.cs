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
        /// <param name="configuration">The IConfiguration instance to extend.</param>
        /// <param name="sectionName">The name of the configuration section to retrieve. 
        /// If null, defaults to the constant section name defined in <seealso cref="Constant.HttpClientSection"/>.</param>
        /// <returns>The configuration section corresponding to the specified section name.</returns>
        public static IConfiguration GetHttpClient(
            this IConfiguration configuration,
            string? sectionName = default)
            => configuration.GetSection(sectionName ?? Constant.HttpClientSection);
    }
}
