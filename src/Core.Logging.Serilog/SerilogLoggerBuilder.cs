using Microsoft.Extensions.Configuration;
using Serilog;

namespace Core.Logging.Serilog
{
    /// <summary>
    /// Represents a builder for configuring and creating a Serilog logger.
    /// Implements the <see cref="ILoggerBuilder"/> interface.
    /// </summary>
    public class SerilogLoggerBuilder : ILoggerBuilder
    {
        private LoggerConfiguration _loggerConfiguration;
        private bool _isBuilt;

        private SerilogLoggerBuilder()
        {
            _loggerConfiguration = new LoggerConfiguration();
        }

        /// <summary>
        /// Creates a new instance of <see cref="SerilogLoggerBuilder"/>
        /// with <see cref="LoggerConfigurationExtensions.WithDefaultConfiguration(LoggerConfiguration)"/>
        /// and from configuration.
        /// </summary>
        /// <param name="configuration">The configuration object to read settings from.</param>
        /// <returns>A new instance of <see cref="SerilogLoggerBuilder"/>.</returns>
        public static SerilogLoggerBuilder CreateBuilder(IConfiguration configuration)
        {
            LoggerConfiguration loggerConfiguration = new LoggerConfiguration()
                .WithDefaultConfiguration()
                .ReadFrom.Configuration(configuration);

            SerilogLoggerBuilder builder = new()
            {
                _loggerConfiguration = loggerConfiguration
            };

            return builder;
        }

        /// <summary>
        /// Configures the logger with the specified settings.
        /// </summary>
        /// <param name="configure">An action to configure the logger settings.</param>
        /// <returns>The current instance of <see cref="SerilogLoggerBuilder"/>.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the logger has already been built.</exception>

        public SerilogLoggerBuilder Configure(Action<LoggerConfiguration> configure)
        {
            if (_isBuilt)
            {
                throw new InvalidOperationException("Logger has already been built.");
            }

            configure.Invoke(_loggerConfiguration);

            return this;
        }

        /// <inheritdoc/>
        public void Build()
        {
            if (_isBuilt)
            {
                return;
            }

            Log.Logger = _loggerConfiguration.CreateLogger();
            Log.Information("Serilog initialized.");

            _isBuilt = true;
        }

        /// <inheritdoc/>
        public void Shutdown()
        {
            Log.Information("Flushing all logs.");
            Log.CloseAndFlush();
        }
    }
}
