namespace Core.HttpClient
{
    /// <summary>
    /// Provides abstraction for an Http client that simplifies making requests.
    /// </summary>
    public interface IBaseHttpClient
    {
        /// <summary>
        /// Sends an Http GET request to the specified URI and returns the deserialized result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result to be deserialized from the Http response.</typeparam>
        /// <param name="uri">The target URI for the GET request.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation. If the operation is successful,
        /// the task result contains the deserialized response of type <typeparamref name="TResult"/>;
        /// otherwise, it may be <c>null</c>.
        /// </returns>
        Task<TResult?> GetAsync<TResult>(string uri, CancellationToken cancellationToken);

        /// <summary>
        /// Sends an Http POST request to the specified URI with the specified data and returns the deserialized result.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request data to be sent.</typeparam>
        /// <typeparam name="TResult">The type of the result to be deserialized from the Http response.</typeparam>
        /// <param name="uri">The target URI for the POST request.</param>
        /// <param name="data">The data to be included in the POST request body.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation. If the operation is successful,
        /// the task result contains the deserialized response of type <typeparamref name="TResult"/>;
        /// otherwise, it may be <c>null</c>.
        /// </returns>
        Task<TResult?> PostAsync<TRequest, TResult>(string uri, TRequest data, CancellationToken cancellationToken);
    }
}
