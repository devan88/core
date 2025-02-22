using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Core.HttpClient.Authorization.Extensions
{
    /// <summary>
    /// Provides extension methods for the http client builder.
    /// </summary>
    public static class HttpClientBuilderExtension
    {
        /// <summary>
        /// Extension method to add a bearer token auth handler to an IHttpClientBuilder.
        /// </summary>
        /// <param name="builder">The IHttpClientBuilder to which the bearer token auth handler will be added.</param>
        /// <param name="configuration">The IConfiguration instance used to retrieve authorization settings.</param>
        /// <param name="configurationPath">Optional http authorization configuration path.</param>
        /// <returns>The updated IHttpClientBuilder with the bearer token auth handler added.</returns>
        public static IHttpClientBuilder WithBearerTokenAuthHandler(
            this IHttpClientBuilder builder,
            IConfiguration configuration,
            string? configurationPath = null)
        {
            IConfiguration configurationSection = configuration.GetHttpClientAuthorization(configurationPath);

            BearerTokenAuthConfiguration? authorizationSection = configurationSection.Get<BearerTokenAuthConfiguration>();

            if (authorizationSection is not null && authorizationSection.CloudProvider is not CloudProvider.None)
            {
                builder.AddHttpMessageHandler(sp =>
                {
                    CloudProvider cloudProvider = authorizationSection!.CloudProvider;
                    IOAuthTokenProviderFactory factory = sp.GetRequiredService<IOAuthTokenProviderFactory>();
                    IOAuthTokenProviderService provider = factory.CreateOAuthTokenProviderService(configurationSection, cloudProvider);
                    return new BearerTokenAuthHandler(provider);
                });
            }

            return builder;
        }

        /// <summary>
        /// Extension method to add a Basic authentication handler to an IHttpClientBuilder.
        /// </summary>
        /// <param name="builder">The IHttpClientBuilder to which the Basic authentication handler will be added.</param>
        /// <param name="configuration">The IConfiguration instance used to retrieve authorization settings.</param>
        /// <param name="configurationPath">Optional http authorization configuration path.</param>
        /// <returns>The updated IHttpClientBuilder with the Basic authentication handler added.</returns>
        public static IHttpClientBuilder WithBasicAuthHandler(
            this IHttpClientBuilder builder,
            IConfiguration configuration,
            string? configurationPath = null)
        {
            BasicAuthConfiguration? basicAuthConfig = configuration
                .GetHttpClientAuthorization(configurationPath)
                .Get<BasicAuthConfiguration>();

            if (basicAuthConfig is not null
                && string.IsNullOrEmpty(basicAuthConfig.Username)
                && string.IsNullOrEmpty(basicAuthConfig.Password))
            {
                builder.AddHttpMessageHandler(sp =>
                {
                    return new BasicAuthHandler(Options.Create(basicAuthConfig));
                });
            }

            return builder;
        }

        /// <summary>
        /// Extension method to add a Api Key authentication handler to an IHttpClientBuilder.
        /// </summary>
        /// <param name="builder">The IHttpClientBuilder to which the Api Key authentication handler will be added.</param>
        /// <param name="configuration">The IConfiguration instance used to retrieve authorization settings.</param>
        /// <param name="configurationPath">Optional http authorization configuration path.</param>
        /// <returns>The updated IHttpClientBuilder with the Api Key authentication handler added.</returns>
        public static IHttpClientBuilder WithApiKeyAuthHandler(
            this IHttpClientBuilder builder,
            IConfiguration configuration,
            string? configurationPath = null)
        {
            ApiKeyAuthConfiguration? apiKeyAuthConfig = configuration
                .GetHttpClientAuthorization(configurationPath)
                .Get<ApiKeyAuthConfiguration>();

            if (apiKeyAuthConfig is not null
                && string.IsNullOrEmpty(apiKeyAuthConfig.HeaderName)
                && string.IsNullOrEmpty(apiKeyAuthConfig.ApiKey))
            {
                builder.AddHttpMessageHandler(sp =>
                {
                    return new ApiKeyAuthHandler(Options.Create(apiKeyAuthConfig));
                });
            }

            return builder;
        }

        /// <summary>
        /// Extension method to add a Api Key authentication handler to an IHttpClientBuilder.
        /// </summary>
        /// <param name="builder">The IHttpClientBuilder to which the Api Key authentication handler will be added.</param>
        /// <param name="configuration">The IConfiguration instance used to retrieve authorization settings.</param>
        /// <param name="configurationPath">Optional http authorization configuration path.</param>
        /// <returns>The updated IHttpClientBuilder with the Api Key authentication handler added.</returns>
        public static IHttpClientBuilder WithClientCertificateAuthHandler(
            this IHttpClientBuilder builder,
            IConfiguration configuration,
            string? configurationPath = null)
        {
            ClientCertificateAuthConfiguration? clientCertificateAuthConfig = configuration
                .GetHttpClientAuthorization(configurationPath)
                .Get<ClientCertificateAuthConfiguration>();

            if (clientCertificateAuthConfig is not null
                && string.IsNullOrEmpty(clientCertificateAuthConfig.PathToCertificate))
            {
                builder.ConfigurePrimaryHttpMessageHandler(sp =>
                {
                    return new ClientCertificateAuthHandler(Options.Create(clientCertificateAuthConfig));
                });
            }

            return builder;
        }

        /// <summary>
        /// Extension method to add a windows authentication handler to an IHttpClientBuilder.
        /// </summary>
        /// <param name="builder">The IHttpClientBuilder to which the windows authentication handler will be added.</param>
        /// <param name="configuration">The IConfiguration instance used to retrieve authorization settings.</param>
        /// <param name="configurationPath">Optional http authorization configuration path.</param>
        /// <returns>The updated IHttpClientBuilder with the windows authentication handler added.</returns>
        public static IHttpClientBuilder WithWindowsAuthHandler(
            this IHttpClientBuilder builder,
            IConfiguration configuration,
            string? configurationPath = null)
        {
            WindowsAuthConfiguration windowsAuthConfig = configuration
                .GetHttpClientAuthorization(configurationPath)
                .Get<WindowsAuthConfiguration>() ?? new WindowsAuthConfiguration();

            builder.ConfigurePrimaryHttpMessageHandler(sp =>
            {
                return new WindowsAuthHandler(Options.Create(windowsAuthConfig));
            });

            return builder;
        }
    }
}
