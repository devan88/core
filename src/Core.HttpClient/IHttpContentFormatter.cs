namespace Core.HttpClient
{
    /// <summary>
    /// Represents a formatter for serializing and deserializing HTTP content.
    /// </summary>
    public interface IHttpContentFormatter
    {
        /// <summary>
        /// Serializes the specified data into an <see cref="HttpContent"/> object.
        /// </summary>
        /// <typeparam name="T">The type of the data to serialize.</typeparam>
        /// <param name="data">The data to serialize.</param>
        /// <returns>An <see cref="HttpContent"/> object containing the serialized data.</returns>
        HttpContent GetContent<T>(T data);

        /// <summary>
        /// Asynchronously deserializes the specified <see cref="HttpContent"/> into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize into.</typeparam>
        /// <param name="data">The <see cref="HttpContent"/> to deserialize.</param>
        /// <param name="cancellationToken">A <see cref="CancellationToken"/> to monitor for cancellation requests.</param>
        /// <returns>
        /// A task that represents the asynchronous deserialization operation.
        /// The task result contains the deserialized object or <c>null</c> if the deserialization fails.
        /// </returns>
        Task<T?> DeserializeAsync<T>(HttpContent data, CancellationToken cancellationToken);
    }
}
