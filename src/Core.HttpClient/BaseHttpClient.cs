using System.Net.Http.Headers;
using Microsoft.Extensions.Logging;

namespace Core.HttpClient
{
    /// <summary>
    /// Base class to support http client request.
    /// </summary>
    public class BaseHttpClient : IBaseHttpClient
    {
        private readonly System.Net.Http.HttpClient _httpClient;

        private readonly IHttpContentFormatterStrategy _httpContentFormatterStrategy;

        private readonly IEnumerable<IHttpContentFormatter> _httpContentFormatters;

        private readonly ILogger<BaseHttpClient> _logger;

        public BaseHttpClient(
            System.Net.Http.HttpClient httpClient,
            IEnumerable<IHttpContentFormatter> httpContentFormatters,
            IHttpContentFormatterStrategy httpContentFormatterStrategy,
            ILogger<BaseHttpClient> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

            _httpContentFormatters
                = httpContentFormatters ?? throw new ArgumentNullException(nameof(httpContentFormatters));

            _httpContentFormatterStrategy =
                httpContentFormatterStrategy ?? throw new ArgumentNullException(nameof(httpContentFormatterStrategy));

            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<HttpResult<TResult?>> GetAsync<TResult>(string uri, CancellationToken cancellationToken)
        {
            return await SendRequestAsync<TResult>(uri, HttpMethod.Get, null, null, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<HttpResult<TResult?>> PostAsync<TRequest, TResult>(string uri, TRequest requestData, string? requestContentType, CancellationToken cancellationToken)
        {
            return await SendRequestAsync<TResult>(uri, HttpMethod.Post, requestData, requestContentType, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task<HttpResult<TResult?>> DeleteAsync<TResult>(string uri, CancellationToken cancellationToken)
        {
            return await SendRequestAsync<TResult>(uri, HttpMethod.Delete, null, null, cancellationToken);
        }

        private async Task<HttpResult<TResult?>> SendRequestAsync<TResult>(string uri, HttpMethod method, object? requestData, string? requestContentType, CancellationToken cancellationToken)
        {
            _logger.LogDebug("{Method} request to:<{Uri}> with request data:<{@RequestData}>", method, uri, requestData);

            using HttpRequestMessage httpRequestMessage = new(method, uri);

            foreach (string? mediaTypes in _httpContentFormatters.Select(f => f.MediaTypes.First()))
            {
                httpRequestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaTypes));
            }

            httpRequestMessage.Content = await GetContentAsync(requestData, requestContentType, cancellationToken)
                .ConfigureAwait(false);

            using HttpResponseMessage response = await _httpClient
                .SendAsync(httpRequestMessage, HttpCompletionOption.ResponseHeadersRead, cancellationToken)
                .ConfigureAwait(false);

            string responseContent = await response.Content
                    .ReadAsStringAsync(cancellationToken)
                    .ConfigureAwait(false);

            return await HandleResponseAsync<TResult>(response, uri, responseContent, cancellationToken);
        }

        private async Task<HttpContent?> GetContentAsync(
            object? requestData,
            string? requestContentType,
            CancellationToken cancellationToken)
        {
            if (requestData is not null)
            {
                requestContentType ??= _httpContentFormatterStrategy.DefaultMediaTypeHeaderValue.MediaType!;

                IHttpContentFormatter httpContentFormatter = _httpContentFormatterStrategy
                .GetHttpContentFormatter(_httpContentFormatters, new MediaTypeWithQualityHeaderValue(requestContentType));

                return await httpContentFormatter
                    .SerializeAsync(requestData, cancellationToken)
                    .ConfigureAwait(false);
            }

            return null;
        }

        private async Task<HttpResult<TResult?>> HandleResponseAsync<TResult>(
            HttpResponseMessage response,
            string uri,
            string responseContent,
            CancellationToken cancellationToken)
        {
            if (TryHandleError(response, uri, responseContent, out HttpResult<TResult?>? result))
            {
                return result!;
            }

            if (string.IsNullOrEmpty(responseContent))
            {
                return new HttpResult<TResult?> { IsSuccess = true };
            }

            _logger.LogDebug("Response message:<{@Response}>", responseContent);

            return await HandleSuccessAsync<TResult>(response, cancellationToken)
                .ConfigureAwait(false);
        }

        private bool TryHandleError<TResult>(
            HttpResponseMessage response,
            string uri,
            string responseContent,
            out HttpResult<TResult?>? result)
        {
            try
            {
                response.EnsureSuccessStatusCode();
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "<{Uri}> returned an error.", uri);
                result = new HttpResult<TResult?>()
                {
                    IsSuccess = false,
                    HttpError = new HttpError()
                    {
                        StatusCode = (int)(ex.StatusCode ?? response.StatusCode),
                        Message = responseContent
                    }
                };
                return true;
            }

            result = null;
            return false;
        }

        private async Task<HttpResult<TResult?>> HandleSuccessAsync<TResult>(
            HttpResponseMessage response,
            CancellationToken cancellationToken)
        {
            IHttpContentFormatter httpContentFormatter = _httpContentFormatterStrategy
                .GetHttpContentFormatter(_httpContentFormatters, response.Content.Headers.ContentType);

            TResult? responseData = await httpContentFormatter
                .DeserializeAsync<TResult>(response.Content, cancellationToken)
                .ConfigureAwait(false);

            return new HttpResult<TResult?>()
            {
                IsSuccess = true,
                Data = responseData
            };
        }
    }
}
