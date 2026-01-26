using Core.HttpClient.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core.HttpClient.Extensions
{
    /// <summary>
    /// Provides extension method to register http clients.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Adds a Http client to the service collection with the name from <see cref="HttpClientOptions.Name"/>.
        /// </summary>
        /// <typeparam name="TClient">The type of the client.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="configureHttpClientOptions">configuration for http client options.</param>
        /// <param name="baseHttpClientFactory">
        /// Optional factory to create <see cref="IBaseHttpClient"/>.
        /// Defaulted to <see cref="BaseHttpClient"/>.
        /// </param>
        /// <returns>The Http client builder.</returns>
        public static IBaseHttpClientBuilder AddStandardBaseHttpClient<TClient>(
            this IServiceCollection services,
            Action<HttpClientOptions> configureHttpClientOptions,
            Func<IServiceProvider, IBaseHttpClient>? baseHttpClientFactory = null)
            where TClient : class
        {
            return services.AddStandardBaseHttpClient<TClient, TClient>(
                configureHttpClientOptions,
                baseHttpClientFactory);
        }

        /// <summary>
        /// Adds a Http client to the service collection with the name from <see cref="HttpClientOptions.Name"/>.
        /// </summary>
        /// <typeparam name="TClient">The type of the client.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration"><see cref="IConfiguration"/> section for http client options.</param>
        /// <param name="baseHttpClientFactory">
        /// Optional factory to create <see cref="IBaseHttpClient"/>.
        /// Defaulted to <see cref="BaseHttpClient"/>.
        /// </param>
        /// <returns>The Http client builder.</returns>
        public static IBaseHttpClientBuilder AddStandardBaseHttpClient<TClient>(
            this IServiceCollection services,
            IConfiguration configuration,
            Func<IServiceProvider, IBaseHttpClient>? baseHttpClientFactory = null)
            where TClient : class
        {
            return services.AddStandardBaseHttpClient<TClient, TClient>(
                configuration,
                baseHttpClientFactory);
        }

        /// <summary>
        /// Adds a Http client to the service collection with the name from <see cref="HttpClientOptions.Name"/>.
        /// </summary>
        /// <typeparam name="TClientService">The type of the client.</typeparam>
        /// <typeparam name="TClientImplementation">The implementation type of the client.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="baseHttpClientFactory">
        /// Optional factory to create <see cref="IBaseHttpClient"/>.
        /// Defaulted to <see cref="BaseHttpClient"/>.
        /// </param>
        /// <param name="configureHttpClientOptions">configuration for http client options.</param>
        /// <returns>The Http client builder.</returns>
        public static IBaseHttpClientBuilder AddStandardBaseHttpClient<TClientService, TClientImplementation>(
            this IServiceCollection services,
            Action<HttpClientOptions> configureHttpClientOptions,
            Func<IServiceProvider, IBaseHttpClient>? baseHttpClientFactory = null)
            where TClientService : class
            where TClientImplementation : class, TClientService
        {
            HttpClientOptions httpClientOptions = configureHttpClientOptions.Invoke();

            return services.AddStandardBaseHttpClient<TClientService, TClientImplementation>(
                httpClientOptions,
                baseHttpClientFactory);
        }


        /// <summary>
        /// Adds a Http client to the service collection with the name from <see cref="HttpClientOptions.Name"/>.
        /// </summary>
        /// <typeparam name="TClientService">The type of the client.</typeparam>
        /// <typeparam name="TClientImplementation">The implementation type of the client.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="baseHttpClientFactory">
        /// Optional factory to create <see cref="IBaseHttpClient"/>.
        /// Defaulted to <see cref="BaseHttpClient"/>.
        /// </param>
        /// <param name="configuration"><see cref="IConfiguration"/> section for http client options.</param>
        /// <returns>The Http client builder.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static IBaseHttpClientBuilder AddStandardBaseHttpClient<TClientService, TClientImplementation>(
            this IServiceCollection services,
            IConfiguration configuration,
            Func<IServiceProvider, IBaseHttpClient>? baseHttpClientFactory = null)
            where TClientService : class
            where TClientImplementation : class, TClientService
        {

            HttpClientOptions httpClientOptions = configuration.Get<HttpClientOptions>()
                ?? throw new ArgumentNullException(nameof(configuration), "HttpClient configuration cannot be found");

            return services.AddStandardBaseHttpClient<TClientService, TClientImplementation>(
                httpClientOptions,
                baseHttpClientFactory);
        }

        /// <summary>
        /// Registers a client of type <typeparamref name="TClientImplementation"/> with <see cref="IBaseHttpClient"/> dependency injected.
        /// </summary>
        /// <typeparam name="TClientService">The client type to be registered.</typeparam>
        /// <typeparam name="TClientImplementation">The type of the implementation to be registered.</typeparam>
        /// <param name="services">The service collection to register with.</param>
        /// <param name="httpClientConfiguration">An object containing configuration settings for the HttpClient.</param>
        /// <param name="baseHttpClientFactory">
        /// An optional factory function to create an <see cref="IBaseHttpClient"/>. 
        /// If not provided, a default implementation via <see cref="IBaseHttpClientFactory.Create(string)"/> is used.
        /// </param>
        /// <returns>
        /// The updated <see cref="IServiceCollection"/> instance.
        /// </returns>
        public static IBaseHttpClientBuilder AddStandardBaseHttpClient<TClientService, TClientImplementation>(
            this IServiceCollection services,
            HttpClientOptions httpClientConfiguration,
            Func<IServiceProvider, IBaseHttpClient>? baseHttpClientFactory = null)
            where TClientService : class
            where TClientImplementation : class, TClientService
        {
            ObjectFactory clientFactory = ActivatorUtilities
                .CreateFactory(typeof(TClientImplementation), [typeof(IBaseHttpClient),]);

            services.TryAddSingleton<IBaseHttpClientFactory, DefaultBaseHttpClientFactory>();
            services.AddOptions<BaseHttpClientOptions>();

            IHttpClientBuilder httpClientBuilder =
                services
                .AddHttpClientFormatterStrategy()
                .AddHttpClient(httpClientConfiguration.Name, client =>
                {
                    client.BaseAddress = httpClientConfiguration.BaseAddress;
                    foreach (KeyValuePair<string, string> kvp in httpClientConfiguration.Headers)
                    {
                        client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
                    }
                })
                .AddTypedClient<TClientService>((httpClient, sp) =>
                {
                    IBaseHttpClientFactory factory = sp.GetRequiredService<IBaseHttpClientFactory>();

                    IBaseHttpClient baseHttpClient = baseHttpClientFactory?.Invoke(sp)
                    ?? factory.Create(httpClientConfiguration.Name);

                    return (TClientImplementation)clientFactory(sp, [baseHttpClient]);
                })
                .WithStandardResilienceHandler()
                .WithStandardHedgingHandler();

            return new DefaultBaseHttpClientBuilder(services, httpClientBuilder)
                .AddJsonHttpContentFormatter()
                .AddHttpContentFormatter<XmlHttpContentFormatter>();
        }

        /// <summary>
        /// Adds http client formatter strategy <see cref="DefaultHttpContentFormatterStrategy"/>.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddHttpClientFormatterStrategy(this IServiceCollection services)
        {
            services.TryAddSingleton<IHttpContentFormatterStrategy, DefaultHttpContentFormatterStrategy>();

            return services;
        }
    }
}
