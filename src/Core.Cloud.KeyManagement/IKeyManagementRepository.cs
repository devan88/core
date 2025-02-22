namespace Core.Cloud.KeyManagement
{
    /// <summary>
    /// Interface that supports retrieving secrets from cloud providers. 
    /// </summary>
    public interface IKeyManagementRepository
    {
        /// <summary>
        /// Gets the secret from the provided key.
        /// </summary>
        /// <param name="key">The key that contains the secret.</param>
        /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
        /// <returns>The secret value.</returns>
        public Task<string> GetSecretAsync(string key, CancellationToken cancellationToken);
    }
}
