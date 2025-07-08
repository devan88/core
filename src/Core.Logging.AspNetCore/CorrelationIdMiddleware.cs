using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Core.Logging.AspNetCore
{
    /// <summary>
    /// Initializes a new instance of the CorrelationIdMiddleware class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="correlationIdService">The service used to generate correlation IDs.</param>
    /// <param name="logger">The logger used to log correlation IDs and requests.</param>
    public sealed class CorrelationIdMiddleware(
        RequestDelegate next,
        ICorrelationIdService correlationIdService,
        ILogger<CorrelationIdMiddleware> logger)
    {
        private readonly RequestDelegate _next =
            next ?? throw new ArgumentNullException(nameof(next));

        private readonly ICorrelationIdService _correlationIdService =
            correlationIdService ?? throw new ArgumentNullException(nameof(correlationIdService));

        private readonly ILogger<CorrelationIdMiddleware> _logger =
            logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>
        /// Invokes the middleware and generates a correlation ID for the current request.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            // Generate a new Correlation ID
            string correlationId = _correlationIdService.GetCorrelationId();

            if (!context.Request.Headers.ContainsKey(Constants.CorrelationIdHeader))
            {
                context.Request.Headers[Constants.CorrelationIdHeader] = correlationId;
            }

            // Log the Correlation ID
            _logger.LogInformation("CorrelationId:<{CorrelationId}> - Handling request:<{Path}>", correlationId, context.Request.Path);

            // Create a logging scope with the Correlation ID
            using (_logger.BeginScopeWithCorrelationId(correlationId))
            {
                await _next(context); // Call the next middleware in the pipeline
            }
        }
    }
}
