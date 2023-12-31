using Core.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Hosting;

namespace Core.Extensions.Configuration
{
    public static class UserSecretsConfigurationExtensions
    {
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

        public static bool TryRemoveUserSecrets(this IConfigurationBuilder config)
        {
            var userSecrets = config.Sources
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
