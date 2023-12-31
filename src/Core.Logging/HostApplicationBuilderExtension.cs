using Core.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting
{
    public static class HostApplicationBuilderExtension
    {
        private static readonly string DiagnosticsLogger = "DiagnosticsLogger";

        public static IHostApplicationBuilder AddDiagnosticsLogger<T>(
            this IHostApplicationBuilder hostBuilder,
            ILoggingBuilder loggingBuilder)
        {
            return hostBuilder.AddDiagnosticsLogger(loggingBuilder, typeof(T));
        }

        public static IHostApplicationBuilder AddDiagnosticsLogger(
            this IHostApplicationBuilder hostBuilder,
            ILoggingBuilder loggingBuilder,
            Type type)
        {
            var provider = loggingBuilder.Services.BuildServiceProvider();
            var logger = provider
                .GetRequiredService<IDiagnosticsLogger>()
                .CreateLogger(type);
            logger.LogInformation("Adding Diagnostics Logger {@logger}", logger.GetType());
            if (hostBuilder.Properties.ContainsKey(DiagnosticsLogger))
            {
                hostBuilder.Properties[DiagnosticsLogger] = logger;
            }
            else
            {
                hostBuilder.Properties.Add(DiagnosticsLogger, logger);
            }
            return hostBuilder;
        }

        public static ILogger GetDiagnosticsLogger(this IHostApplicationBuilder hostBuilder)
        {
            return hostBuilder.Properties[DiagnosticsLogger] as ILogger;
        }
    }
}
