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
        /// the task <see cref="HttpResult{TResult}"/> contains the deserialized response of type <typeparamref name="TResult"/>;
        /// otherwise, it may be <c>null</c>.
        /// </returns>
        Task<HttpResult<TResult?>> GetAsync<TResult>(string uri, CancellationToken cancellationToken);

        /// <summary>
        /// Sends an Http POST request to the specified URI with the specified data and returns the deserialized result.
        /// </summary>
        /// <typeparam name="TRequest">The type of the request data to be sent.</typeparam>
        /// <typeparam name="TResult">The type of the result to be deserialized from the Http response.</typeparam>
        /// <param name="uri">The target URI for the POST request.</param>
        /// <param name="requestData">The data to be included in the POST request body.</param>
        /// <param name="requestContentType">Optional content type format to serialize the request data.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation. If the operation is successful,
        /// the task <see cref="HttpResult{TResult}"/> contains the deserialized response of type <typeparamref name="TResult"/>;
        /// otherwise, it may be <c>null</c>.
        /// </returns>
        Task<HttpResult<TResult?>> PostAsync<TRequest, TResult>(string uri, TRequest requestData, string? requestContentType, CancellationToken cancellationToken);

        /// <summary>
        /// Sends an Http DELETE request to the specified URI and returns the deserialized result.
        /// </summary>
        /// <typeparam name="TResult">The type of the result to be deserialized from the Http response.</typeparam>
        /// <param name="uri">The target URI for the DELETE request.</param>
        /// <param name="cancellationToken">
        /// A <see cref="CancellationToken"/> to observe while waiting for the task to complete.
        /// </param>
        /// <returns>
        /// A task representing the asynchronous operation. If the operation is successful,
        /// the task <see cref="HttpResult{TResult}"/> contains the deserialized response of type <typeparamref name="TResult"/>;
        /// otherwise, it may be <c>null</c>.
        /// </returns>
        Task<HttpResult<TResult?>> DeleteAsync<TResult>(string uri, CancellationToken cancellationToken);
    }
}
