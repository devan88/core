namespace Core.Cloud.Storage
{
    /// <summary>
    /// Defines a contract for a storage repository that provides methods for managing storage items.
    /// </summary>
    public interface IStorageRepository
    {
        /// <summary>
        /// Asynchronously creates a path in the storage system.
        /// </summary>
        /// <param name="path">The path to be created.</param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// Defaults to <see cref="CancellationToken.None"/>.
        /// </param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public Task CreatePathAsync(string path, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously downloads an item from the storage based on the specified filter.
        /// </summary>
        /// <param name="filter">The filter criteria to locate the storage item.</param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// Defaults to <see cref="CancellationToken.None"/>.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the downloaded <see cref="StorageItem"/>.
        /// </returns>
        public Task<StorageItem> DownloadItemAsync(StorageItemFilter filter, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously uploads an item to the storage.
        /// </summary>
        /// <param name="storageItem">The storage item to upload.</param>
        /// <param name="createPath">
        /// Indicates whether to create the path if it does not exist.
        /// Defaults to <c>false</c>.
        /// </param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// Defaults to <see cref="CancellationToken.None"/>.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result indicates whether the upload was successful.
        /// </returns>
        public Task<bool> UploadItemAsync(StorageItem storageItem, bool createPath = false, CancellationToken cancellationToken = default);

        /// <summary>
        /// Asynchronously generates a signed URL for accessing a storage item
        /// with specified access permissions and duration.
        /// </summary>
        /// <param name="filter">The filter criteria to locate the storage item.</param>
        /// <param name="storageItemAccess">The access level for the storage item.</param>
        /// <param name="duration">The duration for which the signed URL is valid.</param>
        /// <param name="cancellationToken">
        /// A token to monitor for cancellation requests.
        /// Defaults to <see cref="CancellationToken.None"/>.
        /// </param>
        /// <returns>
        /// A task that represents the asynchronous operation.
        /// The task result contains the generated signed URL as a <see cref="Uri"/>.
        /// </returns>
        public Task<Uri> GenerateItemSignedUrlAsync(StorageItemFilter filter, StorageItemAccess storageItemAccess, TimeSpan duration, CancellationToken cancellationToken = default);
    }
}
