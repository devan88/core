using Core.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Logging
{
    public static class LoggingBuilderExtensions
    {
        public static ILoggingBuilder AddDiagnostics(this ILoggingBuilder builder)
        {
            builder.Services.AddSingleton<IDiagnosticsLogger, DiagnosticsLogger>();
            return builder;
        }
    }
}
