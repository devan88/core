using Microsoft.Extensions.Logging;

namespace Core.HttpClient
{
    /// <summary>
    /// Base abstract class to support http client request.
    /// </summary>
    public class BaseHttpClient(
        System.Net.Http.HttpClient httpClient,
        IHttpContentFormatterFactory httpContentFormatterFactory,
        ILogger<BaseHttpClient> logger)
        : IBaseHttpClient
    {
        private readonly System.Net.Http.HttpClient _httpClient = httpClient
            ?? throw new ArgumentNullException(nameof(httpClient));

        private readonly IHttpContentFormatterFactory _httpContentFormatterFactory = httpContentFormatterFactory
            ?? throw new ArgumentNullException(nameof(httpContentFormatterFactory));

        private readonly ILogger<BaseHttpClient> _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));

        /// <inheritdoc/>
        public async Task<TResult?> GetAsync<TResult>(
            string uri,
            CancellationToken cancellationToken)
        {
            _logger.LogDebug("GetAsync request message with: {Uri}", uri);

            using HttpResponseMessage response = await _httpClient
                .GetAsync(uri, cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            IHttpContentFormatter httpContentFormatter = _httpContentFormatterFactory
                .CreateHttpContentFormatter(_httpClient.DefaultRequestHeaders.Accept);

            _logger.LogDebug("GetAsync response message: {@Response}", await response.Content.ReadAsStringAsync(cancellationToken));

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
            _logger.LogDebug("PostAsync request message with: {Uri}", uri);

            IHttpContentFormatter httpContentFormatter = _httpContentFormatterFactory
                .CreateHttpContentFormatter(_httpClient.DefaultRequestHeaders.Accept);

            using HttpContent content = httpContentFormatter.GetContent(data);

            using HttpResponseMessage response = await _httpClient
                .PostAsync(uri, content, cancellationToken)
                .ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            _logger.LogDebug("PostAsync response message: {@Response}", await response.Content.ReadAsStringAsync(cancellationToken));

            return await httpContentFormatter
                .DeserializeAsync<TResult>(response.Content, cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
