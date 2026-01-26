namespace Core.HttpClient.Authorization
{
    /// <summary>
    /// Provides a mechanism for retrieving OAuth access tokens using client credentials.
    /// </summary>
    public interface IOAuthTokenProvider
    {
        /// <summary>
        /// The provider unique identifier; Example: ClientId.
        /// This is so that multiple providers can be used for different clients.
        /// Currently used in <see cref="CacheOAuthTokenProvider"/> to get the implementation.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Retrieves an access token that can be used to authenticate with protected resources.
        /// The access token is obtained using the client credentials flow.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>A task that completes with the <see cref="OAuthToken"/>.</returns>
        Task<OAuthToken> GetAccessTokenAsync(CancellationToken cancellationToken);
    }
}
