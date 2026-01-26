using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;

namespace Core.Caching
{
    /// <summary>
    /// Cache service that uses memory cache and distributed cache together.
    /// Retrieves distributed cache and saves to memory cache for quicker retrieval.
    /// </summary>
    public sealed class CacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IDistributedCache _distributedCache;
        private readonly ISerializer _serializer;
        private readonly ILogger<CacheService> _logger;

        public CacheService(
            IMemoryCache memoryCache,
            IDistributedCache distributedCache,
            ISerializer serializer,
            ILogger<CacheService> logger)
        {
            _memoryCache = memoryCache;
            _distributedCache = distributedCache;
            _serializer = serializer;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task StoreAsync<T>(Cache<T> cache, CancellationToken cancellationToken)
        {
            byte[] bytes = await _serializer.SerializeAsync(cache.Value, cancellationToken);

            try
            {
                await _distributedCache.SetAsync(
                cache.Key,
                bytes,
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = cache.ExpiresIn
                },
                cancellationToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Redis set failed for key=<{Key}>", cache.Key);
            }
            

            _memoryCache.Set(cache.Key, bytes, cache.ExpiresIn);
        }

        /// <inheritdoc/>
        public async ValueTask<(bool hasValue, T? value)> GetValueAsync<T>(
            string key,
            CancellationToken cancellationToken)
        {
            bool hasValue = _memoryCache.TryGetValue(key, out byte[]? bytes);

            if (!hasValue)
            {
                try
                {
                    bytes = await _distributedCache
                    .GetAsync(key, cancellationToken)
                    .ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Redis get failed for key=<{Key}>", key);
                }

                hasValue = bytes is not null;
            }

            T? token = hasValue ? await _serializer.DeserializeAsync<T>(bytes!, cancellationToken) : default;

            return (hasValue, token);
        }

        /// <inheritdoc/>
        public async Task RemoveAsync(string key, CancellationToken cancellationToken)
        {
            _memoryCache.Remove(key);
            await _distributedCache.RemoveAsync(key, cancellationToken).ConfigureAwait(false);
        }
    }
}
