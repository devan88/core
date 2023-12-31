namespace Core.Cloud.Storage
{
    public interface IStorageRepository
    {
        public Task CreatePathAsync(string path, CancellationToken cancellationToken = default);
        public Task<StorageItem> DownloadItemAsync(StorageItemFilter filter, CancellationToken cancellationToken = default);
        public Task<bool> UploadItemAsync(StorageItem storageItem, bool createPath = false, CancellationToken cancellationToken = default);
        public Task<Uri> GenerateItemSignedUrlAsync(StorageItemFilter filter, StorageItemAccess storageItemAccess, TimeSpan duration, CancellationToken cancellationToken = default);
    }
}