using Microsoft.Extensions.DependencyInjection;

namespace Core.HttpClient
{
    public interface IBaseHttpClientBuilder
    {
        public IHttpClientBuilder HttpClientBuilder { get; }

        IBaseHttpClientBuilder AddHttpContentFormatter<THttpContentFormatter>()
            where THttpContentFormatter : class, IHttpContentFormatter;

        IBaseHttpClientBuilder AddHttpContentFormatter<THttpContentFormatter, TOptions>(Action<TOptions>? configure)
            where THttpContentFormatter : class, IHttpContentFormatter
            where TOptions : class, new();
    }
}
