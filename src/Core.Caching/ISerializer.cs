namespace Core.Caching
{
    /// <summary>
    /// Defines a contract for a serializer that provides methods to serialize and deserialize objects.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Asynchronously serializes an object into a byte array.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="value">The object to serialize.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the serialized byte array.
        /// </returns>
        Task<byte[]> SerializeAsync<T>(T value, CancellationToken cancellationToken);

        /// <summary>
        /// Asynchronously deserializes a byte array back into an object of the specified type.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="bytes">The byte array containing the serialized object.</param>
        /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the deserialized object.
        /// </returns>
        Task<T> DeserializeAsync<T>(byte[] bytes, CancellationToken cancellationToken);
    }
}
