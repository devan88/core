using Microsoft.Extensions.Options;

namespace Core.HttpClient.Authorization
{
    internal sealed class ApiKeyAuthHandler : DelegatingHandler
    {
        public readonly ApiKeyAuthConfiguration _apiKeyAuthConfiguration;

        public ApiKeyAuthHandler(IOptions<ApiKeyAuthConfiguration> options)
        {
            _apiKeyAuthConfiguration = options.Value;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Add the API key header to the request.
            if (!request.Headers.Contains(_apiKeyAuthConfiguration.HeaderName))
            {
                request.Headers.Add(_apiKeyAuthConfiguration.HeaderName, _apiKeyAuthConfiguration.ApiKey);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
