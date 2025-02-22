namespace Core.HttpClient.Authorization
{
    /// <summary>
    /// Provides a mechanism for retrieving OAuth access tokens using client credentials.
    /// </summary>
    public interface IOAuthTokenProviderService
    {
        /// <summary>
        /// Retrieves an access token that can be used to authenticate with protected resources.
        /// The access token is obtained using the client credentials flow.
        /// </summary>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>A task that completes with the access token as a string.</returns>
        Task<string> GetAccessTokenAsync(CancellationToken cancellationToken);
    }
}
