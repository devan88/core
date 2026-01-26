using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace Core.HttpClient.Authorization.Azure
{
    /// <summary>
    /// Azure OAuth token implementation.
    /// </summary>
    public sealed class AzureOAuthTokenProvider : IOAuthTokenProvider
    {
        private readonly ILogger<AzureOAuthTokenProvider> _logger;
        private readonly AzureAuthConfiguration _azureAuthConfiguration;

        /// <inheritdoc/>
        public string Id => _azureAuthConfiguration.ClientId;

        public AzureOAuthTokenProvider(
            ILogger<AzureOAuthTokenProvider> logger,
            IOptions<AzureAuthConfiguration> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _azureAuthConfiguration = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc/>
        public async Task<OAuthToken> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            IConfidentialClientApplication app = ConfidentialClientApplicationBuilder
                .Create(_azureAuthConfiguration.ClientId)
                .WithClientSecret(_azureAuthConfiguration.ClientSecret)
                .WithTenantId(_azureAuthConfiguration.TenantId)
                .Build();

            _logger.LogInformation("Getting access token for clientId=<{ClientId}>", _azureAuthConfiguration.ClientId);

            AuthenticationResult result = await app
                .AcquireTokenForClient(_azureAuthConfiguration.Scopes)
                .ExecuteAsync(cancellationToken)
                .ConfigureAwait(false);

            _logger.LogInformation("Received access token for clientId=<{ClientId}>", _azureAuthConfiguration.ClientId);

            OAuthToken oauthToken = MapToOAuthTokenResponse(result);

            return oauthToken;
        }

        private static OAuthToken MapToOAuthTokenResponse(AuthenticationResult result)
        {
            return new OAuthToken()
            {
                AccessToken = result.AccessToken,
                ExpiresOn = result.ExpiresOn,
                TokenType = result.TokenType,
            };
        }
    }
}
