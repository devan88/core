using System.Xml.Linq;
using Core.HttpClient.Authorization.Aws;
using Core.HttpClient.Authorization.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.HttpClient.Authorization.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IServiceCollection"/> to register OAuth token provider services.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Adds <see cref="HttpOAuthTokenProvider"/> to the service collection.
        /// </summary>
        /// <typeparam name="TOptions">The type of options used for OAuth configuration.</typeparam>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="configuration">The configuration object containing OAuth settings.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddHttpAuthTokenProvider<TOptions>(
            this IServiceCollection services,
            IConfiguration configuration)
            where TOptions : OAuthConfiguration
        {
            services
                .ConfigureAndValidate<TOptions>(configuration)
                .AddHttpClient<IOAuthTokenProvider, HttpOAuthTokenProvider>((httpclient, sp) =>
                {
                    TOptions options = sp.GetRequiredService<IOptionsSnapshot<TOptions>>().Get(Options.DefaultName);

                    ILogger<HttpOAuthTokenProvider> logger = sp
                    .GetRequiredService<ILogger<HttpOAuthTokenProvider>>();

                    return new HttpOAuthTokenProvider(logger, Options.Create(options), httpclient);
                });

            return services;
        }

        /// <summary>
        /// Adds <see cref="HttpOAuthTokenProvider"/> with a specified name to the service collection.
        /// </summary>
        /// <typeparam name="TOptions">The type of options used for OAuth configuration.</typeparam>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="configuration">The configuration object containing OAuth settings.</param>
        /// <param name="name">The name for the HTTP client.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddHttpAuthTokenProvider<TOptions>(
            this IServiceCollection services,
            IConfiguration configuration,
            string name)
            where TOptions : OAuthConfiguration
        {
            services.AddHttpClient(name);
            services.AddOAuthTokenProvider<TOptions, HttpOAuthTokenProvider>(configuration, name, sp =>
            {
                System.Net.Http.HttpClient httpClient = sp
                    .GetRequiredService<IHttpClientFactory>()
                    .CreateClient(name);

                return ([typeof(System.Net.Http.HttpClient)], [httpClient]);
            });

            return services;
        }

        /// <summary>
        /// Adds <typeparamref name="TProvider"/> with a specified name to the service collection.
        /// </summary>
        /// <typeparam name="TOptions">The type of options used for OAuth configuration.</typeparam>
        /// <typeparam name="TProvider">The type of the token provider to register.</typeparam>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="configuration">The configuration object containing OAuth settings.</param>
        /// <param name="name">The name for the token provider.</param>
        /// <param name="argumentsFactory">An optional factory to create additional arguments for the provider.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddOAuthTokenProvider<TOptions, TProvider>(
            this IServiceCollection services,
            IConfiguration configuration,
            string name,
            Func<IServiceProvider, (Type[]? argumentTypes, object?[]? arguments)>? argumentsFactory = null)
            where TProvider : class, IOAuthTokenProvider
            where TOptions : OAuthConfiguration
        {

            services
                .ConfigureAndValidate<TOptions>(configuration, name)
                .AddKeyedTransient<IOAuthTokenProvider, TProvider>(name, (sp, key) =>
                {
                    (Type[]? argumentTypes, object?[]? arguments)? args = argumentsFactory?.Invoke(sp);

                    TOptions options = sp.GetRequiredService<IOptionsSnapshot<TOptions>>().Get(name);

                    Type[] argumentTypes = [typeof(IOptions<TOptions>), .. args?.argumentTypes ?? [],];
                    object?[] arguments = [Options.Create(options), .. args?.arguments ?? [],];


                    ObjectFactory factory = ActivatorUtilities.CreateFactory(typeof(TProvider), argumentTypes);
                    return (TProvider)factory(sp, arguments);
                });

            return services;
        }

        /// <summary>
        /// Adds <see cref="AzureOAuthTokenProvider"/> to the service collection
        /// and decorates it with <see cref="CacheOAuthTokenProvider"/>.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="configuration">The configuration object containing <see cref="AzureAuthConfiguration"/> settings.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddAzureAuthTokenProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .ConfigureAndValidate<AzureAuthConfiguration>(configuration)
                .AddTransient<IOAuthTokenProvider, AzureOAuthTokenProvider>()
                .Decorate<IOAuthTokenProvider, CacheOAuthTokenProvider>();

            return services;
        }

        /// <summary>
        /// Adds <see cref="AzureOAuthTokenProvider"/> with a specified name to the service collection
        /// and decorates it with <see cref="CacheOAuthTokenProvider"/>.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="configuration">The configuration object containing <see cref="AzureAuthConfiguration"/> settings.</param>
        /// <param name="name">The name for the provider.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddAzureAuthTokenProvider(
            this IServiceCollection services,
            IConfiguration configuration,
            string name)
        {
            services
                .AddOAuthTokenProvider<AzureAuthConfiguration, AzureOAuthTokenProvider>(configuration, name)
                .Decorate<IOAuthTokenProvider, CacheOAuthTokenProvider>(name);

            return services;
        }

        /// <summary>
        /// Adds <see cref="HttpOAuthTokenProvider"/> with <see cref="AwsAuthConfiguration"/> to the service collection.
        /// Decorates it with <see cref="CacheOAuthTokenProvider"/>.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="configuration">The configuration object containing AWS OAuth settings.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddAwsAuthTokenProvider(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddHttpAuthTokenProvider<AwsAuthConfiguration>(configuration)
                .Decorate<IOAuthTokenProvider, CacheOAuthTokenProvider>();

            return services;
        }

        /// <summary>
        /// Adds <see cref="HttpOAuthTokenProvider"/> with <see cref="AwsAuthConfiguration"/> to the service collection.
        /// Decorates it with <see cref="CacheOAuthTokenProvider"/>.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="configuration">The configuration object containing AWS OAuth settings.</param>
        /// <param name="name">The name for the AWS token provider.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddAwsAuthTokenProvider(
            this IServiceCollection services,
            IConfiguration configuration,
            string name)
        {
            services
                .AddHttpAuthTokenProvider<AwsAuthConfiguration>(configuration, name)
                .Decorate<IOAuthTokenProvider, CacheOAuthTokenProvider>(name);

            return services;
        }

        /// <summary>
        /// Configures and validates options for a specified type.
        /// </summary>
        /// <typeparam name="TOptions">The type of options to configure.</typeparam>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="configuration">The configuration object containing settings.</param>
        /// <param name="name">The optional name for the options.</param>
        /// <returns>The updated service collection.</returns>

        private static IServiceCollection ConfigureAndValidate<TOptions>(
            this IServiceCollection services,
            IConfiguration configuration,
            string? name = null)
            where TOptions : class
        {
            services
                .AddOptionsWithValidateOnStart<TOptions>(name)
                .Bind(configuration)
                .ValidateDataAnnotations();

            return services;
        }
    }
}
