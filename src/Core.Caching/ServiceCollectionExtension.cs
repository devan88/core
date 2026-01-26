using Microsoft.Extensions.Caching.StackExchangeRedis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Caching
{
    /// <summary>
    /// Provides extension methods for <see cref="IServiceCollection"/> to register cache services.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Adds the Memory and Redis cache provider to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <param name="configuration">The configuration to get configuration for redis.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddCacheServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            void configureOptions(RedisCacheOptions options) => configuration.Bind(options);

            return services.AddCacheServices(configureOptions);
        }

        /// <summary>
        /// Registers <see cref="MessagePackSerializer"/> and <see cref="CacheService"/>
        /// to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The service collection to which the caching services will be added.</param>
        /// <param name="configureOptions">An action to configure the Redis cache options.</param>
        /// <returns>The updated service collection with caching services registered.</returns>
        public static IServiceCollection AddCacheServices(
            this IServiceCollection services,
            Action<RedisCacheOptions> configureOptions)
        {
            services.AddMemoryCache();
            services.AddStackExchangeRedisCache(configureOptions);
            services.AddTransient<ISerializer, MessagePackSerializer>();
            services.AddTransient<ICacheService, CacheService>();
            return services;
        }
    }
}
