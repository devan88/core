using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Core.Hosting.AspNetCore.Swagger
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection TryAddSwaggerGen(
            this IServiceCollection services,
            SwaggerOptions swaggerOptions,
            ILogger logger)
        {
            try
            {
                logger.LogInformation("TryAddSwaggerGen {@Options}", swaggerOptions);
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
