using System.Net.Http.Headers;
using Core.HttpClient.Formatters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;

namespace Core.HttpClient.Extensions
{
    /// <summary>
    /// Provides extension method to register http clients.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Adds a Http client to the service collection.
        /// </summary>
        /// <typeparam name="TClient">The type of the client.</typeparam>
        /// <param name="services">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="configurationPath">Optional http configuration path.</param>
        /// <param name="configureHttpStandardResilienceOptions">Optional configuration for the Http standard resilience options.</param>
        /// <param name="configureRoutingStrategyBuilder">Optional configuration for the routing strategy builder.</param>
        /// <returns>The http client builder.</returns>
        /// <exception cref="ArgumentNullException">Throws when htpp client configuration cannot be found.</exception>
        public static IHttpClientBuilder AddStandardHttpClient<TClient>(
            this IServiceCollection services,
            IConfiguration configuration,
            string? configurationPath = null,
            Action<HttpStandardResilienceOptions>? configureHttpStandardResilienceOptions = null,
            Action<IRoutingStrategyBuilder>? configureRoutingStrategyBuilder = null)
            where TClient : class
        {
            IConfiguration httpClientConfigurationSection = configuration.GetHttpClient(configurationPath);

            HttpClientConfiguration httpClientConfiguration = httpClientConfigurationSection.Get<HttpClientConfiguration>()
                ?? throw new ArgumentNullException(nameof(configuration), "HttpClient configuration cannot be found");

            return services
                .AddHttpClientFormatter()
                .AddHttpClient<TClient>(client =>
                {
                    client.BaseAddress = httpClientConfiguration.BaseAddress;
                    foreach (KeyValuePair<string, string> kvp in httpClientConfiguration.Headers)
                    {
                        client.DefaultRequestHeaders.Add(kvp.Key, kvp.Value);
                    }
                })
                .WithStandardResilienceHandler(configureHttpStandardResilienceOptions)
                .WithStandardHedgingHandler(configureRoutingStrategyBuilder);
        }

        /// <summary>
        /// Adds all the supported http client formatters and DefaultHttpContentFormatterFactory.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The service collection.</returns>
        public static IServiceCollection AddHttpClientFormatter(this IServiceCollection services)
        {
            return services
                .AddTransient<JsonHttpContentFormatter>()
                .AddTransient<XmlHttpContentFormatter>()
                .AddSingleton<IHttpContentFormatterFactory, DefaultHttpContentFormatterFactory>();
        }
    }
}
