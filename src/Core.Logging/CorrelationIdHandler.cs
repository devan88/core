namespace Core.Logging
{
    /// <summary>
    /// Initializes a new instance of the CorrelationIdHandler class.
    /// </summary>
    /// <param name="correlationIdService">The service used to generate correlation IDs.</param>
    public sealed class CorrelationIdHandler(ICorrelationIdService correlationIdService) : DelegatingHandler
    {
        private readonly ICorrelationIdService _correlationIdService =
            correlationIdService ?? throw new ArgumentNullException(nameof(correlationIdService));

        /// <summary>
        /// Sends the HTTP request to the inner handler.
        /// If the request does not contain a correlation ID header, a new correlation ID is generated and added to the request.
        /// </summary>
        /// <param name="request">The HTTP request to send.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(Constants.CorrelationIdHeader))
            {
                request.Headers.Add(Constants.CorrelationIdHeader, _correlationIdService.GetCorrelationId());
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
