using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Extensions.Configuration
{
    /// <summary>
    /// Provides extension methods for adding key-per-file configuration to an IConfigurationBuilder.
    /// </summary>
    public static class KeyPerFileConfigurationBuilderExtensions
    {
        /// <summary>
        /// Tries to add key-per-file configuration to the specified configuration builder.
        /// </summary>
        /// <param name="builder">The configuration builder to which the key-per-file configuration will be added.</param>
        /// <param name="logger">An optional logger for logging information and errors.</param>
        /// <param name="keyPerFilePath">
        /// The path to the directory containing the key-per-file configuration.
        /// Defaults to "/mnt/secrets".
        /// </param>
        /// <returns>The updated configuration builder.</returns>
        public static IConfigurationBuilder TryAddKeyPerFile(
            this IConfigurationBuilder builder,
            ILogger? logger = null,
            string keyPerFilePath = "/mnt/secrets")
        {
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), keyPerFilePath);
                logger?.LogInformation("Using Key-per-file from:<{Path}>", path);
                builder.AddKeyPerFile(path, true, true);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Error adding KeyPerFile");
            }
            return builder;
        }
    }
}
