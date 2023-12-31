using Microsoft.Extensions.Logging;

namespace Core.Logging
{
    public sealed class DiagnosticsLogger : IDiagnosticsLogger
    {

        public ILogger<T> CreateLogger<T>()
        {
            return CreateLogger(typeof(T)) as ILogger<T>;
        }

        public ILogger CreateLogger<T>(T type)
            where T : Type
        {
            return LoggerFactory.Create(builder =>
            {
                builder.AddConsole();
            }).CreateLogger(type);
        }
    }
}
