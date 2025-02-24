namespace Core.HttpClient
{
    /// <summary>
    /// Base abstract class to support http client request.
    /// </summary>
    public class BaseHttpClient(
        System.Net.Http.HttpClient httpClient,
        IHttpContentFormatterFactory httpContentFormatterFactory)
        : IBaseHttpClient
    {
        private readonly System.Net.Http.HttpClient _httpClient = httpClient
            ?? throw new ArgumentNullException(nameof(httpClient));

        private readonly IHttpContentFormatterFactory _httpContentFormatterFactory = httpContentFormatterFactory
            ?? throw new ArgumentNullException(nameof(httpContentFormatterFactory));

        /// <inheritdoc/>
        public async Task<TResult?> GetAsync<TResult>(
            string uri,
            CancellationToken cancellationToken)
        {
            using HttpResponseMessage response = await _httpClient
                .GetAsync(uri, cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            IHttpContentFormatter httpContentFormatter = _httpContentFormatterFactory
                .CreateHttpContentFormatter(_httpClient.DefaultRequestHeaders.Accept);

            return await httpContentFormatter
                .DeserializeAsync<TResult>(response.Content, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<TResult?> PostAsync<TRequest, TResult>(
            string uri,
            TRequest data,
            CancellationToken cancellationToken)
        {
            IHttpContentFormatter httpContentFormatter = _httpContentFormatterFactory
                .CreateHttpContentFormatter(_httpClient.DefaultRequestHeaders.Accept);

            using HttpContent content = httpContentFormatter.GetContent(data);

            using HttpResponseMessage response = await _httpClient
                .PostAsync(uri, content, cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return await httpContentFormatter
                .DeserializeAsync<TResult>(response.Content, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
