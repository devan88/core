using Core.HttpClient.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Logging;

namespace Core.HttpClient.Extensions
{
    /// <summary>
    /// Provides extension method to register http clients.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Adds a Http client to the service collection with the name from <see cref="HttpClientConfiguration.Name"/>.
        /// </summary>
        /// <typeparam name="TClient">The type of the client.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="baseHttpClientFactory">
        /// Optional factory to create <see cref="IBaseHttpClient"/>.
        /// Defaulted to <see cref="BaseHttpClient"/>.
        /// </param>
        /// <param name="configurationPath">Optional http configuration path.</param>
        /// <param name="configureHttpStandardResilienceOptions">Optional configuration for the Http standard resilience options.</param>
        /// <param name="configureRoutingStrategyBuilder">Optional configuration for the routing strategy builder.</param>
        /// <returns>The Http client builder.</returns>
        /// <exception cref="ArgumentNullException">Throws when htpp client configuration cannot be found.</exception>
        public static IHttpClientBuilder AddStandardBaseHttpClient<TClient>(
            this IServiceCollection services,
            IConfiguration configuration,
            string? configurationPath = null,
            Func<IServiceProvider, IBaseHttpClient>? baseHttpClientFactory = null,
            Action<HttpStandardResilienceOptions>? configureHttpStandardResilienceOptions = null,
            Action<IRoutingStrategyBuilder>? configureRoutingStrategyBuilder = null)
            where TClient : class
        {
            return services.AddStandardBaseHttpClient<TClient, TClient>(
                configuration,
                configurationPath,
                baseHttpClientFactory,
                configureHttpStandardResilienceOptions,
                configureRoutingStrategyBuilder);
        }

        /// <summary>
        /// Adds a Http client to the service collection with the name from <see cref="HttpClientConfiguration.Name"/>.
        /// </summary>
        /// <typeparam name="TClientService">The type of the client.</typeparam>
        /// <typeparam name="TClientImplementation">The implementation type of the client.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="baseHttpClientFactory">
        /// Optional factory to create <see cref="IBaseHttpClient"/>.
        /// Defaulted to <see cref="BaseHttpClient"/>.
        /// </param>
        /// <param name="configurationPath">Optional http configuration path.</param>
        /// <param name="configureHttpStandardResilienceOptions">Optional configuration for the Http standard resilience options.</param>
        /// <param name="configureRoutingStrategyBuilder">Optional configuration for the routing strategy builder.</param>
        /// <returns>The Http client builder.</returns>
        /// <exception cref="ArgumentNullException">Throws when htpp client configuration cannot be found.</exception>
        public static IHttpClientBuilder AddStandardBaseHttpClient<TClientService, TClientImplementation>(
            this IServiceCollection services,
            IConfiguration configuration,
            string? configurationPath = null,
            Func<IServiceProvider, IBaseHttpClient>? baseHttpClientFactory = null,
            Action<HttpStandardResilienceOptions>? configureHttpStandardResilienceOptions = null,
            Action<IRoutingStrategyBuilder>? configureRoutingStrategyBuilder = null)
            where TClientService : class
            where TClientImplementation : class, TClientService
        {
            IConfiguration httpClientConfigurationSection = configuration.GetHttpClient(configurationPath);

            HttpClientConfiguration httpClientConfiguration = httpClientConfigurationSection.Get<HttpClientConfiguration>()
                ?? throw new ArgumentNullException(nameof(configuration), "HttpClient configuration cannot be found");

            return services
                .AddClientWithBaseHttpClient<TClientService, TClientImplementation>(httpClientConfiguration.Name, baseHttpClientFactory)
                .AddHttpClientFormatter()
                .AddStandardHttpClient(httpClientConfiguration)
                .WithStandardResilienceHandler(configureHttpStandardResilienceOptions)
                .WithStandardHedgingHandler(configureRoutingStrategyBuilder);
        }

        /// <summary>
        /// Adds all the supported http client formatters and the <see cref="DefaultHttpContentFormatterFactory"/>.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddHttpClientFormatter(this IServiceCollection services)
        {
            services.TryAddTransient<JsonHttpContentFormatter>();
            services.TryAddTransient<XmlHttpContentFormatter>();
            services.TryAddSingleton<IHttpContentFormatterFactory, DefaultHttpContentFormatterFactory>();

            return services;
        }

        /// <summary>
        /// Registers an HttpClient with the name from <see cref="HttpClientConfiguration.Name"/>.
        /// </summary>
        /// <param name="services">The service collection to register the HttpClient with.</param>
        /// <param name="httpClientConfiguration">An object containing configuration settings for the HttpClient.</param>
        /// <returns>
        /// An <see cref="IHttpClientBuilder"/> instance used to further configure the registered HttpClient.
        /// </returns>
        public static IHttpClientBuilder AddStandardHttpClient(
            this IServiceCollection services,
            HttpClientConfiguration httpClientConfiguration)
        {
            return services.AddHttpClient(httpClientConfiguration.Name, client =>
            {
                client.BaseAddress = httpClientConfiguration.BaseAddress;
                foreach (KeyValuePair<string, string> kvp in httpClientConfiguration.Headers)
                {
                    client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
                }
            });
        }

        /// <summary>
        /// Registers a client of type <typeparamref name="TClientImplementation"/> with <see cref="IBaseHttpClient"/> dependency injected.
        /// </summary>
        /// <typeparam name="TClientService">The client type to be registered.</typeparam>
        /// <typeparam name="TClientImplementation">The type of the implementation to be registered.</typeparam>
        /// <param name="services">The service collection to register with.</param>
        /// <param name="httpClientName">
        /// The http client name registered from
        /// <see cref="HttpClientFactoryServiceCollectionExtensions.AddHttpClient(IServiceCollection, string)" />.
        /// </param>
        /// <param name="baseHttpClientFactory">
        /// An optional factory function to create an <see cref="IBaseHttpClient"/>. 
        /// If not provided, a default implementation via <see cref="CreateDefaultBaseHttpClient(IServiceProvider, string)"/> is used.
        /// </param>
        /// <returns>
        /// The updated <see cref="IServiceCollection"/> instance.
        /// </returns>
        public static IServiceCollection AddClientWithBaseHttpClient<TClientService, TClientImplementation>(
            this IServiceCollection services,
            string httpClientName,
            Func<IServiceProvider, IBaseHttpClient>? baseHttpClientFactory = null)
            where TClientService : class
            where TClientImplementation : class, TClientService
        {
            return services
                .AddTransient<TClientService>(sp =>
                {
                    IBaseHttpClient baseHttpClient = baseHttpClientFactory?.Invoke(sp)
                        ?? CreateDefaultBaseHttpClient(sp, httpClientName);

                    ObjectFactory factory = ActivatorUtilities.CreateFactory(typeof(TClientImplementation), [typeof(IBaseHttpClient),]);
                    return (TClientImplementation)factory(sp, [baseHttpClient]);
                });
        }

        /// <summary>
        /// Creates a new instance of <see cref="BaseHttpClient"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="httpClientName">The http client configureed name.</param>
        /// <returns>a instance of <see cref="BaseHttpClient"/>.</returns>
        private static BaseHttpClient CreateDefaultBaseHttpClient(IServiceProvider serviceProvider, string httpClientName)
        {
            IHttpClientFactory httpClientFactory = serviceProvider.GetRequiredService<IHttpClientFactory>();

            IHttpContentFormatterFactory httpContentFormatterFactory = serviceProvider
                .GetRequiredService<IHttpContentFormatterFactory>();

            ILogger<BaseHttpClient> logger = serviceProvider.GetRequiredService<ILogger<BaseHttpClient>>();

            System.Net.Http.HttpClient httpClient = httpClientFactory.CreateClient(httpClientName);

            return new BaseHttpClient(httpClient, httpContentFormatterFactory, logger);
        }
    }
}
