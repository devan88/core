using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;

namespace Core.Logging.Serilog
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder ConfigureSerilog(
            this ILoggingBuilder builder,
            Action<LoggerConfiguration>? configureLogger = null)
        {
            LoggerConfiguration loggerConfiguration = GetLoggerConfiguration();
            configureLogger?.Invoke(loggerConfiguration);

            return builder.ConfigureSerilog(loggerConfiguration);

        }

        public static ILoggingBuilder ConfigureSerilog(this ILoggingBuilder builder, IConfiguration configuration)
        {
            LoggerConfiguration loggerConfiguration = GetLoggerConfiguration().ReadFrom.Configuration(configuration);

            return builder.ConfigureSerilog(loggerConfiguration);
        }

        public static ILoggingBuilder ConfigureSerilog(this ILoggingBuilder builder, LoggerConfiguration loggerConfiguration)
        {
            Logger serilogLogger = loggerConfiguration.CreateLogger();

            Log.Logger = serilogLogger;

            builder.Services.AddSingleton<ILoggerFactory>(sp =>
            {
                return new SerilogLoggerFactory(serilogLogger);
            });

            return builder;
        }

        private static LoggerConfiguration GetLoggerConfiguration()
        {
            return new LoggerConfiguration()
                .WithMinimumLevel()
                .WithEnrichers()
                .WithConsole()
                .WithFile();
        }
    }
}
