using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace Core.HttpClient.Authorization.Azure
{
    /// <summary>
    /// Azure OAuth token implementation.
    /// </summary>
    public sealed class AzureOAuthTokenProviderService : IOAuthTokenProviderService
    {
        private readonly ILogger<AzureOAuthTokenProviderService> _logger;
        private readonly AzureAuthConfiguration _azureAuthConfiguration;

        public AzureOAuthTokenProviderService(
            ILogger<AzureOAuthTokenProviderService> logger,
            IOptions<AzureAuthConfiguration> options)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _azureAuthConfiguration = options.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc/>
        public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken)
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

            return result.AccessToken;
        }
    }
}
