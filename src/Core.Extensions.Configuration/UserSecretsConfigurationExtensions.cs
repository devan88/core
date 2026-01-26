using Core.Extensions.Configuration;
using Core.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;

namespace Core.Extensions.Configuration
{
    /// <summary>
    /// Provides extension methods for adding and managing user secrets in the configuration builder.
    /// </summary>
    public static class UserSecretsConfigurationExtensions
    {
        /// <summary>
        /// Adds user secrets to the configuration builder if the hosting environment is LocalDevelopment.
        /// </summary>
        /// <typeparam name="T">The type used to identify the user secrets.</typeparam>
        /// <param name="builder">The configuration builder to which user secrets will be added.</param>
        /// <param name="hostingEnvironment">The hosting environment to determine if user secrets should be added.</param>
        /// <returns>The updated configuration builder.</returns>
        public static IConfigurationBuilder AddUserSecrets<T>(
            this IConfigurationBuilder builder,
            IHostEnvironment hostingEnvironment)
            where T : class
        {
            if (hostingEnvironment.IsLocalDevelopment())
            {
                builder.AddUserSecrets<T>();
            }
            else
            {
                builder.TryRemoveUserSecrets();
            }

            return builder;
        }

        /// <summary>
        /// Attempts to remove user secrets from the configuration builder.
        /// </summary>
        /// <param name="config">The configuration builder from which to remove user secrets.</param>
        /// <returns><c>true</c> if user secrets were successfully removed; otherwise, <c>false</c>.</returns>
        private static bool TryRemoveUserSecrets(this IConfigurationBuilder config)
        {
            IConfigurationSource? userSecrets = config.Sources
                .Where(cs => cs is JsonConfigurationSource)
                .SingleOrDefault(cs => ((JsonConfigurationSource)cs).Path == "secrets.json");
            if (userSecrets != null)
            {
                return config.Sources.Remove(userSecrets);
            }
            return false;
        }
    }
}
