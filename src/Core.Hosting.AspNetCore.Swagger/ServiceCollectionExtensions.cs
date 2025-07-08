using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Core.Hosting.AspNetCore.Swagger
{
    /// <summary>
    /// Provides extensions for adding SwaggerGen to the IServiceCollection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Attempts to add SwaggerGen to the IServiceCollection using the provided IConfiguration.
        /// </summary>
        /// <param name="services">The IServiceCollection to add SwaggerGen to.</param>
        /// <param name="configuration">The IConfiguration to retrieve Swagger options from.</param>
        /// <param name="logger">The ILogger to log messages with.</param>
        /// <returns>The IServiceCollection with SwaggerGen added, if successful.</returns>
        public static IServiceCollection TryAddSwaggerGen(
            this IServiceCollection services,
            IConfiguration configuration,
            ILogger logger)
        {
            SwaggerOptions swaggerOptions = configuration.Get<SwaggerOptions>() ?? new SwaggerOptions();

            return TryAddSwaggerGen(services, swaggerOptions, logger);
        }

        /// <summary>
        /// Attempts to add SwaggerGen to the IServiceCollection using the provided Action to configure Swagger options.
        /// </summary>
        /// <param name="services">The IServiceCollection to add SwaggerGen to.</param>
        /// <param name="configureSwaggerOptions">The Action to configure Swagger options.</param>
        /// <param name="logger">The ILogger to log messages with.</param>
        /// <returns>The IServiceCollection with SwaggerGen added, if successful.</returns>
        public static IServiceCollection TryAddSwaggerGen(
            this IServiceCollection services,
            Action<SwaggerOptions> configureSwaggerOptions,
            ILogger logger)
        {
            SwaggerOptions swaggerOptions = new();
            configureSwaggerOptions(swaggerOptions);

            return TryAddSwaggerGen(services, swaggerOptions, logger);
        }

        /// <summary>
        /// Attempts to add SwaggerGen to the IServiceCollection using the provided SwaggerOptions.
        /// </summary>
        /// <param name="services">The IServiceCollection to add SwaggerGen to.</param>
        /// <param name="swaggerOptions">The SwaggerOptions to use for configuration.</param>
        /// <param name="logger">The ILogger to log messages with.</param>
        /// <returns>The IServiceCollection with SwaggerGen added, if successful.</returns>
        public static IServiceCollection TryAddSwaggerGen(
            this IServiceCollection services,
            SwaggerOptions swaggerOptions,
            ILogger logger)
        {
            try
            {
                logger.LogInformation("TryAddSwaggerGen {@Options}", swaggerOptions);
                services.AddSingleton(swaggerOptions);
                services.AddSwaggerGen(c =>
                {
                    OpenApiOptions apiOptions = swaggerOptions.SwaggerDoc.OpenApiOptions;
                    c.SwaggerDoc(swaggerOptions.SwaggerDoc.Name, new OpenApiInfo { Title = apiOptions.Title, Version = apiOptions.Version });
                });
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error adding SwaggerGen: {@Exception}", ex);
            }
            return services;
        }
    }
}
