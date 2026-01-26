namespace Core.Caching
{
    /// <summary>
    /// A serializer using MessagePack.
    /// </summary>
    public sealed class MessagePackSerializer : ISerializer
    {
        /// <inheritdoc/>
        public Task<T> DeserializeAsync<T>(byte[] bytes, CancellationToken cancellationToken)
        {
            T value = MessagePack.MessagePackSerializer.Deserialize<T>(
                    bytes,
                    MessagePack.Resolvers.ContractlessStandardResolver.Options,
                    cancellationToken);

            return Task.FromResult(value);
        }

        /// <inheritdoc/>
        public Task<byte[]> SerializeAsync<T>(T value, CancellationToken cancellationToken)
        {
            byte[] bytes = MessagePack.MessagePackSerializer.Serialize(
                value,
                MessagePack.Resolvers.ContractlessStandardResolver.Options,
                cancellationToken);

            return Task.FromResult(bytes);
        }
    }
}
