using AutoMapper;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics;

namespace Core.Cloud.Storage.Azure
{
    public sealed class AzureStorageRepository : IStorageRepository
    {
        private readonly ILogger<AzureStorageRepository> _logger;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IAzureClientFactory<BlobServiceClient> _clientFactory;
        private readonly AzureStorageConfiguration _azureStorageConfiguration;
        private readonly IMapper _mapper;

        public AzureStorageRepository(
            ILogger<AzureStorageRepository> logger,
            IOptions<AzureStorageConfiguration> azureStorageConfiguration,
            BlobServiceClient blobServiceClient,
            IAzureClientFactory<BlobServiceClient> clientFactory,
            IMapper mapper)
        {
            _logger = logger;
            _azureStorageConfiguration = azureStorageConfiguration.Value;
            _blobServiceClient = blobServiceClient;
            _clientFactory = clientFactory;
            _mapper = mapper;
        }

        public async Task CreatePathAsync(string path, CancellationToken cancellationToken = default)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(path);
            await blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
        }

        public async Task<StorageItem> DownloadItemAsync(StorageItemFilter filter, CancellationToken cancellationToken = default)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(filter.Path);
            var blobClient = blobContainerClient.GetBlobClient(filter.Name);
            var blobItem = await blobClient.DownloadStreamingAsync(
                new BlobDownloadOptions()
                {
                    ProgressHandler = filter.ProgressHandler
                }, cancellationToken);
            var storageItem = _mapper.Map<StorageItem>(blobItem.Value);
            return storageItem;
        }

        public async Task<Uri> GenerateItemSignedUrlAsync(
            StorageItemFilter filter,
            StorageItemAccess storageItemAccess,
            TimeSpan duration,
            CancellationToken cancellationToken = default)
        {
            var blobServiceSasClient = _clientFactory.CreateClient(_azureStorageConfiguration.SasClientName);
            var blobClient = blobServiceSasClient.GetBlobContainerClient(filter.Path).GetBlobClient(filter.Name);
            var permission = _mapper.Map<BlobSasPermissions>(storageItemAccess);
            // Get a user delegation key for the Blob service that's valid for the duration
            var userDelegationKey = await blobServiceSasClient.GetUserDelegationKeyAsync(
                null,
                DateTimeOffset.UtcNow.Add(duration),
                cancellationToken);
            var blobSasUri = CreateUserDelegationSasBlob(
                blobClient,
                userDelegationKey,
                duration,
                permission);
            return blobSasUri;
        }

        public async Task<bool> UploadItemAsync(StorageItem storageItem, bool createPath = false, CancellationToken cancellationToken = default)
        {
            if (createPath)
            {
                await CreatePathAsync(storageItem.Path, cancellationToken);
            }

            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(storageItem.Path);
            var blobClient = blobContainerClient.GetBlobClient(storageItem.Name);

            await WatchTaskAsync(
                () => blobClient.UploadAsync(storageItem.Content, _azureStorageConfiguration.BlobUploadOptions, cancellationToken),
                storageItem.Name).ConfigureAwait(false);
            return true;
        }

        private Task<T> WatchTaskAsync<T>(Func<Task<T>> func, string name)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                return func();
            }
            finally
            {
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                string eT = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                   ts.Hours, ts.Minutes, ts.Seconds,
                   ts.Milliseconds / 10);
                _logger.LogDebug("File : {name} Upload Time: {eT}", name, eT);
            }
        }

        private Uri CreateUserDelegationSasBlob(
            BlobClient blobClient,
            UserDelegationKey userDelegationKey,
            TimeSpan duration,
            BlobSasPermissions blobSasPermissions)
        {
            // Create a SAS token for the blob resource
            BlobSasBuilder sasBuilder = new()
            {
                BlobContainerName = blobClient.BlobContainerName,
                BlobName = blobClient.Name,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.Add(duration)
            };

            // Specify the necessary permissions
            sasBuilder.SetPermissions(blobSasPermissions);

            // Add the SAS token to the blob URI
            BlobUriBuilder uriBuilder = new(blobClient.Uri)
            {
                // Specify the user delegation key
                Sas = sasBuilder.ToSasQueryParameters(userDelegationKey, _azureStorageConfiguration.Name)
            };

            return uriBuilder.ToUri();
        }
    }
}