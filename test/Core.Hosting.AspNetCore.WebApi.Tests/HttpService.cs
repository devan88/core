using Core.HttpClient;

namespace Core.Hosting.AspNetCore.WebApi.Tests
{
    public sealed class HttpService
    {
        private readonly IBaseHttpClient _httpClient;
        public HttpService(IBaseHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> GetWeatherDataAsync<T>(CancellationToken cancellationToken)
        {
            return await _httpClient.GetAsync<T>("api/WeatherForecast/Get", cancellationToken);
        }
    }
}
