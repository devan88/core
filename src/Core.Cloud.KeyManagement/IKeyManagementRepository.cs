namespace Core.Cloud.KeyManagement
{
    public interface IKeyManagementRepository
    {
        public Task<string> GetSecretAsync(string key, CancellationToken cancellationToken);
    }
}
