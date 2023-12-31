using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;

namespace Core.Logging.Serilog
{
    public sealed class SerilogDiagnosticsLogger : IDiagnosticsLogger
    {
        public ILogger<T> CreateLogger<T>()
        {
            return (ILogger<T>)CreateLogger(typeof(T));
        }

        public Microsoft.Extensions.Logging.ILogger CreateLogger<T>(T type)
            where T : Type
        {
            var serilogConsoleLogger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            return new SerilogLoggerFactory(serilogConsoleLogger)
                .CreateLogger(type);
        }
    }
}
