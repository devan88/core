using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.HttpClient
{
    internal sealed class DefaultBaseHttpClientBuilder : IBaseHttpClientBuilder
    {
        private readonly IServiceCollection _services;

        public IHttpClientBuilder HttpClientBuilder { get; }

        public DefaultBaseHttpClientBuilder(
            IServiceCollection services,
            IHttpClientBuilder httpClientBuilder)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            HttpClientBuilder = httpClientBuilder ?? throw new ArgumentNullException(nameof(httpClientBuilder));
        }

        public IBaseHttpClientBuilder AddHttpContentFormatter<THttpContentFormatter>()
            where THttpContentFormatter : class, IHttpContentFormatter
        {
            _services.AddTransient<THttpContentFormatter>();

            _services.Configure<BaseHttpClientOptions>(HttpClientBuilder.Name, options =>
            {
                static THttpContentFormatter createFunc(IServiceProvider sp)
                {
                    return sp.GetRequiredService<THttpContentFormatter>();
                }
                options.HttpContentFormatterFactories[typeof(THttpContentFormatter)] = createFunc;
            });

            return this;
        }

        public IBaseHttpClientBuilder AddHttpContentFormatter<THttpContentFormatter, TOptions>(
            Action<TOptions>? configure)
            where THttpContentFormatter : class, IHttpContentFormatter
            where TOptions : class, new()
        {
            ObjectFactory clientFactory = ActivatorUtilities
                .CreateFactory(typeof(THttpContentFormatter), [typeof(TOptions),]);

            _services.AddKeyedSingleton(HttpClientBuilder.Name, (key, sp) =>
            {
                TOptions options = Activator.CreateInstance<TOptions>();
                configure?.Invoke(options);
                return options;
            });

            _services.Configure<BaseHttpClientOptions>(HttpClientBuilder.Name, options =>
            {
                THttpContentFormatter createFunc(IServiceProvider sp)
                {
                    TOptions options = sp.GetRequiredKeyedService<TOptions>(HttpClientBuilder.Name);
                    return (THttpContentFormatter)clientFactory(sp, [options]);
                }

                options.HttpContentFormatterFactories[typeof(THttpContentFormatter)] = createFunc;
            });

            return this;
        }

    }
}
