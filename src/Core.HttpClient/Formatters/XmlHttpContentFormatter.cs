using System.Collections.Immutable;
using System.Xml.Serialization;

namespace Core.HttpClient.Formatters
{
    /// <summary>
    /// Xml formatter.
    /// </summary>
    public sealed class XmlHttpContentFormatter : IHttpContentFormatter
    {
        /// <inheritdoc/>
        public ImmutableHashSet<string> MediaTypes => MediaType.Xml;

        /// <inheritdoc/>
        public async Task<T?> DeserializeAsync<T>(HttpContent data, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(data);

            await using Stream contentStream = await data
                .ReadAsStreamAsync(cancellationToken)
                .ConfigureAwait(false);

            XmlSerializer xmlSerializer = new(typeof(T));

            return await Task.Run(() => (T?)xmlSerializer
            .Deserialize(contentStream), cancellationToken)
                .ConfigureAwait(false); 
        }

        /// <inheritdoc/>
        public Task<HttpContent> SerializeAsync<T>(T data, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(data);

            PushStreamContent pushStreamContent = new(async (stream, _, _) =>
            {
                XmlSerializer xmlSerializer = new(typeof(T));

                await Task.Run(() => xmlSerializer
                .Serialize(stream, data), cancellationToken)
                .ConfigureAwait(false);

                await stream
                .FlushAsync(cancellationToken)
                .ConfigureAwait(false);

                stream.Close();

            }, MediaTypes.First());

            return Task.FromResult<HttpContent>(pushStreamContent);
        }
    }
}
