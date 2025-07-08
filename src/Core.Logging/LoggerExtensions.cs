using Microsoft.Extensions.Logging;

namespace Core.Logging
{
    /// <summary>
    /// Provides <see cref="ILogger"/> extension methods.
    /// </summary>
    public static class LoggerExtensions
    {
        /// <summary>
        /// Begins a new scope with the specified correlation ID.
        /// This allows the correlation ID to be included in all log messages within the scope.
        /// </summary>
        /// <param name="logger">The logger to begin the scope with.</param>
        /// <param name="correlationId">The correlation ID to include in the scope.</param>
        /// <returns>An IDisposable that can be used to end the scope.</returns>
        public static IDisposable? BeginScopeWithCorrelationId(this ILogger logger, string correlationId)
        {
            return logger.BeginScope("CorrelationId:<{CorrelationId}>", correlationId);
        }
    }
}
