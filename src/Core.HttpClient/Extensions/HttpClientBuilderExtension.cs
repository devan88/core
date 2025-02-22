using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http.Resilience;

namespace Core.HttpClient.Extensions
{
    /// <summary>
    /// Provides extension methods for the http client builder.
    /// </summary>
    public static class HttpClientBuilderExtension
    {
        /// <summary>
        /// Add a standard resilience handler to the Http client builder.
        /// </summary>
        /// <param name="builder">The Http client builder.</param>
        /// <param name="configureHttpStandardResilienceOptions">Optional configuration for the http standard resilience options.</param>
        /// <returns>The Http client builder.</returns>
        public static IHttpClientBuilder WithStandardResilienceHandler(
            this IHttpClientBuilder builder,
            Action<HttpStandardResilienceOptions>? configureHttpStandardResilienceOptions = null)
        {
            builder.AddStandardResilienceHandler(configureHttpStandardResilienceOptions ?? (options =>
            {
#if NET9_0_OR_GREATER
                    options.Retry.DisableForUnsafeHttpMethods();
#endif
            }));

            return builder;
        }

        /// <summary>
        /// Add a standard hedging handler to the Http client builder.
        /// </summary>
        /// <param name="builder">The Http client builder.</param>
        /// <param name="configureRoutingStrategyBuilder">Optional configuration for the routing strategy builder.</param>
        /// <returns>The Http client builder.</returns>
        public static IHttpClientBuilder WithStandardHedgingHandler(
            this IHttpClientBuilder builder,
            Action<IRoutingStrategyBuilder>? configureRoutingStrategyBuilder = null)
        {
            builder.AddStandardHedgingHandler(configureRoutingStrategyBuilder ?? (options => { }));

            return builder;
        }
    }
}
