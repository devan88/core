using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.HttpClient.Authorization
{
    /// <summary>
    /// Http OAuth token implementation.
    /// </summary>
    public sealed class HttpOAuthTokenProvider : IOAuthTokenProvider
    {
        private readonly ILogger<HttpOAuthTokenProvider> _logger;
        private readonly OAuthConfiguration _oAuthConfiguration;
        private readonly System.Net.Http.HttpClient _httpClient;

        /// <inheritdoc/>
        public string Id => _oAuthConfiguration.ClientId;

        public HttpOAuthTokenProvider(
            ILogger<HttpOAuthTokenProvider> logger,
            IOptions<OAuthConfiguration> options,
            System.Net.Http.HttpClient httpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _oAuthConfiguration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <inheritdoc/>
        public async Task<OAuthToken> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            // Prepare the required parameters.
            var parameters = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _oAuthConfiguration.ClientId },
                { "client_secret", _oAuthConfiguration.ClientSecret },
                { "scope", string.Join(" ", _oAuthConfiguration.Scopes) }
            };

            // Prepare the request content as application/x-www-form-urlencoded.
            HttpContent content = new FormUrlEncodedContent(parameters);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            // Send the POST request.
            _logger.LogInformation("Getting access token for clientId=<{ClientId}>", _oAuthConfiguration.ClientId);
            HttpResponseMessage response = await _httpClient
                .PostAsync(_oAuthConfiguration.Endpoint, content, cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                throw new ArgumentException($"Error retrieving token: {response.StatusCode}; {errorContent}");
            }

            string json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Received access token for clientId=<{ClientId}>", _oAuthConfiguration.ClientId);

            // Deserialize and return the token response.
            OAuthTokenResponse tokenResponse = JsonSerializer.Deserialize<OAuthTokenResponse>(json)!;
            OAuthToken oAuthToken = MapToOAuthTokenResponse(tokenResponse);

            return oAuthToken;
        }

        private static OAuthToken MapToOAuthTokenResponse(OAuthTokenResponse tokenResponse)
        {
            return new OAuthToken()
            {
                AccessToken = tokenResponse.AccessToken,
                ExpiresOn = DateTimeOffset.UtcNow.AddSeconds(tokenResponse.ExpiresIn),
                TokenType = tokenResponse.TokenType,
            };
        }
    }
}
