using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.HttpClient
{
    internal sealed class DefaultBaseHttpClientFactory : IBaseHttpClientFactory
    {
        private readonly IOptionsMonitor<BaseHttpClientOptions> _optionsMonitor;
        private readonly IServiceProvider _serviceProvider;

        public DefaultBaseHttpClientFactory(
            IOptionsMonitor<BaseHttpClientOptions> optionsMonitor,
            IServiceProvider serviceProvider)
        {
            _optionsMonitor = optionsMonitor;
            _serviceProvider = serviceProvider;
        }

        public IBaseHttpClient Create(string name)
        {
            BaseHttpClientOptions options = _optionsMonitor.Get(name);
            IEnumerable<IHttpContentFormatter> httpContentFormatters = [.. options.HttpContentFormatterFactories.Select(f => f.Value(_serviceProvider))];

            IHttpClientFactory httpClientFactory = _serviceProvider.GetRequiredService<IHttpClientFactory>();
            System.Net.Http.HttpClient httpClient = httpClientFactory.CreateClient(name);

            ILogger<BaseHttpClient> logger = _serviceProvider.GetRequiredService<ILogger<BaseHttpClient>>();

            IHttpContentFormatterStrategy httpContentFormatterFactory = _serviceProvider
                .GetRequiredService<IHttpContentFormatterStrategy>();

            return new BaseHttpClient(httpClient, httpContentFormatters, httpContentFormatterFactory, logger);
        }
    }
}
