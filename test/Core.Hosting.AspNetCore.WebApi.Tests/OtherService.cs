using Core.HttpClient;

namespace Core.Hosting.AspNetCore.WebApi.Tests
{
    public sealed class OtherService
    {
        private readonly IBaseHttpClient _httpClient;
        public OtherService(IBaseHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<T?> GetWeatherDataAsync<T>(CancellationToken cancellationToken)
        {
            return await _httpClient.GetAsync<T>("api/WeatherForecast/Get", cancellationToken);
        }
    }
}
