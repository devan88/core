using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Core.Logging.Serilog
{
    /// <summary>
    /// Provides extension methods for <see cref="IHostBuilder"/>.
    /// </summary>

    public static class HostBuilderExtensions
    {
        /// <summary>
        /// Configures Serilog logging for the host builder using the specified configuration.
        /// </summary>
        /// <param name="hostBuilder">The host builder to extend.</param>
        /// <param name="configuration">The configuration object to read logging settings from.</param>
        /// <param name="configureLogger">An optional action to customize the logger configuration.</param>
        /// <returns>The updated host builder.</returns>
        public static IHostBuilder ConfigureSerilog(
            this IHostBuilder hostBuilder,
            IConfiguration configuration,
            Action<LoggerConfiguration>? configureLogger = null)
        {
            LoggerConfiguration loggerConfiguration = new LoggerConfiguration().ReadFrom.Configuration(configuration);
            configureLogger?.Invoke(loggerConfiguration);

            Log.Logger = loggerConfiguration.CreateLogger();

            return hostBuilder.UseSerilog();
        }
    }
}
