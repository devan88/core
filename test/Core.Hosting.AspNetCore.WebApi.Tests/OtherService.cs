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
            HttpResult<T?> httpResult = await _httpClient.GetAsync<T>("api/v1/WeatherForecast/Get", cancellationToken);
            return httpResult.Data;
        }
    }
}
