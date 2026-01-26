using Core.Extensions.Configuration;
using Core.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Core.Hosting
{
    /// <summary>
    /// Provides methods for building application configuration.
    /// </summary>
    public static class ConfigurationFactory
    {
        /// <summary>
        /// Builds the application configuration based on the following:
        /// 1) appsettings.json
        /// 2) appsettings.environment.json
        /// 3) environment variables
        /// 4) command line arguments
        /// 5) user secrets (environment is Development or LocalDevelopment)
        /// 6) key per file (/mnt/secrets/)
        /// </summary>
        /// <typeparam name="T">The type used to determine user secrets for development environments.</typeparam>
        /// <param name="args">The command-line arguments passed to the application.</param>
        /// <returns>An <see cref="IConfiguration"/> object containing the application configuration.</returns>
        public static IConfiguration BuildConfiguration<T>(string[] args)
        {
            string environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")
                ?? Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ??
                Environments.Production;

            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args);

            if (environment == Environments.Development
                || environment == HostEnvironmentExtensions.LocalDevelopment)
            {
                builder.AddUserSecrets(typeof(T).Assembly, optional: true);
            }

            builder.TryAddKeyPerFile();

            return builder.Build();
        }
    }
}
