namespace Core.HttpClient
{
    public interface IBaseHttpClientFactory
    {
        IBaseHttpClient Create(string name);
    }
}
