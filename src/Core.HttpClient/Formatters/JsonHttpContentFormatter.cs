using System.Collections.Immutable;
using System.Text.Json;

namespace Core.HttpClient.Formatters
{
    /// <summary>
    /// Json formatter.
    /// </summary>
    public sealed class JsonHttpContentFormatter : IHttpContentFormatter
    {
        private readonly JsonSerializerOptions _options;

        /// <inheritdoc/>
        public ImmutableHashSet<string> MediaTypes => MediaType.Json;

        public JsonHttpContentFormatter(JsonSerializerOptions? options = null)
        {
            _options = options ?? JsonOptionsProvider.Default;
        }

        /// <inheritdoc/>
        public async Task<T?> DeserializeAsync<T>(HttpContent data, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(data);

            await using Stream contentStream = await data
                .ReadAsStreamAsync(cancellationToken)
                .ConfigureAwait(false);

            return await JsonSerializer
                .DeserializeAsync<T>(contentStream, _options, cancellationToken)
                .ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public Task<HttpContent> SerializeAsync<T>(T data, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(data);

            PushStreamContent pushStreamContent = new(async (stream, _, _) =>
            {
                await JsonSerializer.SerializeAsync(stream, data, _options, cancellationToken)
                .ConfigureAwait(false);

                await stream.FlushAsync(cancellationToken)
                .ConfigureAwait(false);

                stream.Close();

            }, MediaTypes.First());

            return Task.FromResult<HttpContent>(pushStreamContent);
        }
    }
}
