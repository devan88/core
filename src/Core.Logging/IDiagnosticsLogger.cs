using Microsoft.Extensions.Logging;

namespace Core.Logging
{
    public interface IDiagnosticsLogger
    {
        public ILogger CreateLogger<T>(T type)
            where T : Type;

        public ILogger<T> CreateLogger<T>();
    }
}
