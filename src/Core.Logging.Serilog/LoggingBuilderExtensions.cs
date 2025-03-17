using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Core.Logging.Serilog
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddSerilogDiagnostics(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<IDiagnosticsLogger, SerilogDiagnosticsLogger>();
            return builder;
        }
    }
}
