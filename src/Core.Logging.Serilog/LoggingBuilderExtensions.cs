using Core.Logging;
using Core.Logging.Serilog;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Logging
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
