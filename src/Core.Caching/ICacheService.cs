namespace Core.Caching
{
    /// <summary>
    /// Defines a contract for a caching service that provides methods to get, store, and remove cached values.
    /// </summary>
    public interface ICacheService
    {
        /// <summary>
        /// Asynchronously retrieves a value from the cache by its key.
        /// </summary>
        /// <typeparam name="T">The type of the value to retrieve.</typeparam>
        /// <param name="key">The key associated with the cached value.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains a tuple indicating whether a value was found and the cached value itself.
        /// </returns>
        ValueTask<(bool hasValue, T? value)> GetValueAsync<T>(string key, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously stores a value in the cache.
        /// </summary>
        /// <typeparam name="T">The type of the value to store.</typeparam>
        /// <param name="cache">The cache object containing the value to be stored.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task StoreAsync<T>(Cache<T> cache, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously removes a value from the cache by its key.
        /// </summary>
        /// <param name="key">The key associated with the cached value to remove.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task RemoveAsync(string key, CancellationToken cancellationToken);
    }
}
