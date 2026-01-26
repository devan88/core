using Core.Caching;
using Microsoft.Extensions.Logging;

namespace Core.HttpClient.Authorization
{
    /// <summary>
    /// Cache OAuth token implementation.
    /// </summary>
    public sealed class CacheOAuthTokenProvider : IOAuthTokenProvider
    {
        private readonly ILogger<CacheOAuthTokenProvider> _logger;
        private readonly IOAuthTokenProvider _oAuthTokenProvider;
        private readonly ICacheService _cacheService;

        /// <inheritdoc/>
        public string Id => _oAuthTokenProvider.Id;

        public CacheOAuthTokenProvider(
            ILogger<CacheOAuthTokenProvider> logger,
            IOAuthTokenProvider oAuthTokenProvider,
            ICacheService cachedService)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            _oAuthTokenProvider = oAuthTokenProvider
                ?? throw new ArgumentNullException(nameof(oAuthTokenProvider));

            _cacheService = cachedService
                ?? throw new ArgumentNullException(nameof(cachedService));
        }

        /// <inheritdoc/>
        public async Task<OAuthToken> GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            (bool hasValue, OAuthToken? value) = await _cacheService
                .GetValueAsync<OAuthToken>(Id, cancellationToken)
                .ConfigureAwait(false);

            if (hasValue)
            {
                _logger.LogInformation("Received access token from cache");
                return value!;
            }

            OAuthToken oAuthToken = await _oAuthTokenProvider.GetAccessTokenAsync(cancellationToken);

            await _cacheService.StoreAsync(new Cache<OAuthToken>()
            {
                Key = Id,
                Value = oAuthToken,
                ExpiresIn = oAuthToken.ExpiresOn - DateTimeOffset.UtcNow
            }, cancellationToken);

            return oAuthToken;
        }
    }
}
