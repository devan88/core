namespace Core.Cloud.Storage
{
    /// <summary>
    /// Represents a filter for retrieving storage items based on specific criteria.
    /// </summary>
    public record StorageItemFilter
    {
        /// <summary>
        /// Gets or sets the name of the storage item to filter by.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the path of the storage item to filter by.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets a progress handler to report the progress of operations related to the storage item.
        /// </summary>
        public IProgress<long> ProgressHandler { get; set; }
    }
}
