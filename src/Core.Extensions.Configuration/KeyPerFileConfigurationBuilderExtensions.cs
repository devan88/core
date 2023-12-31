using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Extensions.Configuration
{
    public static class KeyPerFileConfigurationBuilderExtensions
    {

        public static IConfigurationBuilder TryAddKeyPerFile(
            this IConfigurationBuilder builder,
            ILogger logger,
            string keyPerFilePath = "/mnt/secrets")
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), keyPerFilePath);
                logger.LogInformation("Using Key-per-file from: {path}", path);
                builder.AddKeyPerFile(path, true, true);
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding KeyPerFile: {@ex}", ex);
            }
            return builder;
        }
    }
}
