using Azure.Storage;
using Azure.Storage.Blobs.Models;
using Core.Cloud.Configurations.Azure.Storage;

namespace Core.Cloud.Storage.Azure
{
    public class AzureStorageConfiguration : AzureStorage
    {
        public string SasClientName { get; set; } = "Sas";
        public BlobUploadOptions BlobUploadOptions { get; set; } = new BlobUploadOptions()
        {
            TransferOptions = new StorageTransferOptions
            {
                // Set the maximum number of parallel transfer workers
                MaximumConcurrency = 2,

                // Set the initial transfer length to 8 MiB
                InitialTransferSize = 8 * 1024 * 1024,

                // Set the maximum length of a transfer to 4 MiB
                MaximumTransferSize = 4 * 1024 * 1024,
            }
        };
    }
}
