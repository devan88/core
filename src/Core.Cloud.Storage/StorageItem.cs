namespace Core.Cloud.Storage
{
    /// <summary>
    /// Represents an item stored in the storage system, including its metadata and content.
    /// </summary>
    public record StorageItem
    {
        /// <summary>
        /// Gets or sets the name of the storage item.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the path where the storage item is located.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the content type of the storage item (e.g., "image/png", "application/pdf").
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the content of the storage item as a stream.
        /// </summary>
        public Stream Content { get; set; }
    }
}
