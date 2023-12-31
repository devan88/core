using AutoFixture;
using AutoFixture.Xunit2;
using AutoMapper;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NSubstitute;
using System.Text;

namespace Core.Cloud.Storage.Azure.Tests
{
    public class AzureStorageRepositoryTest
    {
        private readonly ILogger<AzureStorageRepository> _logger;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly IAzureClientFactory<BlobServiceClient> _clientFactory;
        private readonly IOptions<AzureStorageConfiguration> _azureStorageConfiguration;
        private readonly IMapper _mapper;

        private AzureStorageRepository _storageRepository;

        public AzureStorageRepositoryTest()
        {

            _logger = Substitute.For<ILogger<AzureStorageRepository>>();
            _blobServiceClient = Substitute.For<BlobServiceClient>();
            _clientFactory = Substitute.For<IAzureClientFactory<BlobServiceClient>>();

            _azureStorageConfiguration = Options.Create(new AzureStorageConfiguration()
            {
                Name = "test",
            });
            _mapper = new MapperConfiguration(cfg =>
                cfg.AddProfile<StorageItemProfile>()
            ).CreateMapper();
        }

        [Theory]
        [InlineData("test")]
        public async Task CreatePathAsync_NewPath_CreatesNewPath(string path)
        {
            // Arrange
            var tokenSource = new CancellationTokenSource();
            var blobContainerClient = Substitute.For<BlobContainerClient>();
            _blobServiceClient.GetBlobContainerClient(path).Returns(blobContainerClient);
            _storageRepository = CreateDefaultStorageRepository();

            // Act
            await _storageRepository.CreatePathAsync(path, tokenSource.Token);

            // Assert
            await blobContainerClient.Received(1).CreateIfNotExistsAsync(cancellationToken: tokenSource.Token);


        }

        [Theory]
        [AutoData]
        public async Task DownloadItemAsync_ExistingItem_ReturnsStream(string path, string name)
        {
            // Arrange
            var tokenSource = new CancellationTokenSource();
            var filter = new StorageItemFilter() { Name = name, Path = path };
            var blobContainerClient = Substitute.For<BlobContainerClient>();
            _blobServiceClient.GetBlobContainerClient(path).Returns(blobContainerClient);
            var blobClient = Substitute.For<BlobClient>();
            blobContainerClient.GetBlobClient(filter.Name).Returns(blobClient);
            var content = new MemoryStream(Encoding.UTF8.GetBytes("testing"));
            var response = Substitute.For<Response>();
            var blobDownloadStreamingResult = BlobsModelFactory.BlobDownloadStreamingResult(content);
            var result = Response.FromValue(blobDownloadStreamingResult, response);
            blobClient.DownloadStreamingAsync(Arg.Any<BlobDownloadOptions>(), tokenSource.Token).Returns(result);
            _storageRepository = CreateDefaultStorageRepository();

            // Act
            var storageItem = await _storageRepository.DownloadItemAsync(filter, tokenSource.Token);

            // Assert
            Assert.NotNull(storageItem.Content);


        }

        [Theory]
        [AutoData]
        public async Task GenerateItemSignedUrlAsync_ReadAccess_ReturnSasUri(
            StorageItemAccess storageItemAccess,
            TimeSpan duration,
            Uri uri)
        {
            // Arrange
            var filter = new StorageItemFilter() { Name = "Test", Path = "Path" };
            var tokenSource = new CancellationTokenSource();
            var blobServiceClient = Substitute.For<BlobServiceClient>();
            var blobClient = Substitute.For<BlobClient>();
            var responseUserDelegationKey = Substitute.For<Response<UserDelegationKey>>();
            var userDelegationKey = BlobsModelFactory.UserDelegationKey("", "", "", "", "", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow);
            responseUserDelegationKey.Value.Returns(userDelegationKey);
            blobClient.Uri.Returns(uri);
            blobServiceClient.CanGenerateAccountSasUri.ReturnsForAnyArgs(true);
            blobServiceClient.GetBlobContainerClient(filter.Path).GetBlobClient(filter.Name).Returns(blobClient);
            blobServiceClient
                .GetUserDelegationKeyAsync(null, Arg.Any<DateTimeOffset>(), tokenSource.Token)
                .Returns(responseUserDelegationKey);
            blobServiceClient.Uri.Returns(uri);
            _clientFactory.CreateClient("Sas").ReturnsForAnyArgs(blobServiceClient);

            _storageRepository = CreateDefaultStorageRepository();

            // Act
            var sasUri = await _storageRepository.GenerateItemSignedUrlAsync(filter, storageItemAccess, duration, tokenSource.Token);

            // Assert
            Assert.NotNull(sasUri);


        }

        [Fact]
        public async Task UploadItemAsync_CreatePath_ReturnSuccess()
        {
            // Arrange
            var fixture = new Fixture();
            var storageItem = fixture.Build<StorageItem>()
                .With(x => x.Content, new MemoryStream(Encoding.UTF8.GetBytes("testing")))
                .Create();
            var tokenSource = new CancellationTokenSource();
            var blobContainerClient = Substitute.For<BlobContainerClient>();
            _blobServiceClient.GetBlobContainerClient(storageItem.Path).Returns(blobContainerClient);
            var blobClient = Substitute.For<BlobClient>();
            blobContainerClient.GetBlobClient(storageItem.Name).Returns(blobClient);
            _storageRepository = CreateDefaultStorageRepository();

            // Act
            var isUploaded = await _storageRepository.UploadItemAsync(storageItem, true, tokenSource.Token);

            // Assert
            Assert.True(isUploaded);
            await blobClient
                .Received(1)
                .UploadAsync(storageItem.Content, _azureStorageConfiguration.Value.BlobUploadOptions, tokenSource.Token);


        }

        private AzureStorageRepository CreateDefaultStorageRepository()
        {
            return new AzureStorageRepository(
                _logger,
                _azureStorageConfiguration,
                _blobServiceClient,
                _clientFactory,
                _mapper);
        }
    }
}