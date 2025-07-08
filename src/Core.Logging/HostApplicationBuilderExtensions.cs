using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Logging
{
    public static class HostApplicationBuilderExtensions
    {
        private const string DiagnosticsLogger = "DiagnosticsLogger";

        public static IHostApplicationBuilder AddDiagnosticsLogger<T>(
            this IHostApplicationBuilder hostBuilder,
            ILoggingBuilder loggingBuilder)
        {
            ServiceProvider provider = loggingBuilder.Services.BuildServiceProvider();

            ILogger<T> logger = provider.GetRequiredService<ILogger<T>>();

            logger.LogInformation("Adding Diagnostics Logger {@Logger}", logger.GetType());

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
            return (ILogger)hostBuilder.Properties[DiagnosticsLogger];
        }
    }
}
