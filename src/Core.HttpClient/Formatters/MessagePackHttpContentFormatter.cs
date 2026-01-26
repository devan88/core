using System.Collections.Immutable;
using MessagePack;

namespace Core.HttpClient.Formatters
{
    internal class MessagePackHttpContentFormatter : IHttpContentFormatter
    {
        public readonly MessagePackSerializerOptions _options;

        public ImmutableHashSet<string> MediaTypes => MediaType.MessagePack;

        public MessagePackHttpContentFormatter(MessagePackSerializerOptions? options = null)
        {
            _options = options ?? MessagePackSerializerOptions.Standard;
        }


        public async Task<T?> DeserializeAsync<T>(HttpContent data, CancellationToken cancellationToken)
        {
            await using Stream contentStream = await data
                .ReadAsStreamAsync(cancellationToken)
                .ConfigureAwait(false);

            return await MessagePackSerializer
                .DeserializeAsync<T>(contentStream, _options, cancellationToken)
                .ConfigureAwait(false);
        }

        public Task<HttpContent> SerializeAsync<T>(T data, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(data);

            PushStreamContent pushStreamContent = new(async (stream, _, _) =>
            {
                await MessagePackSerializer.SerializeAsync(stream, data, _options, cancellationToken)
                .ConfigureAwait(false);

                await stream.FlushAsync(cancellationToken)
                .ConfigureAwait(false);

                stream.Close();

            }, MediaTypes.First());

            return Task.FromResult<HttpContent>(pushStreamContent);
        }
    }
}
