using Azure.Security.KeyVault.Secrets;

namespace Core.Cloud.KeyManagement.Azure
{
    /// <summary>
    /// Azure key vault management repository.
    /// </summary>
    public sealed class AzureKeyManagementRepository : IKeyManagementRepository
    {
        private readonly SecretClient _secretClient;

        public AzureKeyManagementRepository(SecretClient secretClient)
        {
            _secretClient = secretClient;
        }

        /// <inheritdoc/>
        public async Task<string> GetSecretAsync(string key, CancellationToken cancellationToken)
        {
            KeyVaultSecret secret = await _secretClient
                .GetSecretAsync(key, cancellationToken: cancellationToken)
                .ConfigureAwait(false);

            return secret.Value;
        }
    }
}
