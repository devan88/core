using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.HttpClient.Authorization.Aws
{
    /// <summary>
    /// AWS OAuth token implementation.
    /// </summary>
    public sealed class AwsOAuthTokenProviderService : IOAuthTokenProviderService
    {
        private readonly ILogger<AwsOAuthTokenProviderService> _logger;
        private readonly AwsAuthConfiguration _awsAuthConfiguration;
        private readonly System.Net.Http.HttpClient _httpClient;

        public AwsOAuthTokenProviderService(
            ILogger<AwsOAuthTokenProviderService> logger,
            IOptions<AwsAuthConfiguration> options,
            System.Net.Http.HttpClient httpClient)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _awsAuthConfiguration = options.Value ?? throw new ArgumentNullException(nameof(options));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <inheritdoc/>
        public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            // Prepare the required parameters.
            var parameters = new Dictionary<string, string>
            {
                { "grant_type", "client_credentials" },
                { "client_id", _awsAuthConfiguration.ClientId },
                { "client_secret", _awsAuthConfiguration.ClientSecret },
                { "scope", string.Join(" ", _awsAuthConfiguration.Scopes) }
            };

            // Prepare the request content as application/x-www-form-urlencoded.
            HttpContent content = new FormUrlEncodedContent(parameters);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            // Send the POST request.
            _logger.LogInformation("Getting access token for clientId=<{ClientId}>", _awsAuthConfiguration.ClientId);
            HttpResponseMessage response = await _httpClient
                .PostAsync(_awsAuthConfiguration.TokenEndpoint, content, cancellationToken)
                .ConfigureAwait(false);

            if (!response.IsSuccessStatusCode)
            {
                string errorContent = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                throw new ArgumentException($"Error retrieving token: {response.StatusCode}; {errorContent}");
            }

            string json = await response.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogInformation("Received access token for clientId=<{ClientId}>", _awsAuthConfiguration.ClientId);

            // Deserialize and return the token response.
            AwsCognitoTokenResponse? tokenResponse = JsonSerializer.Deserialize<AwsCognitoTokenResponse>(json);

            return tokenResponse?.AccessToken ?? string.Empty;
        }
    }
}
