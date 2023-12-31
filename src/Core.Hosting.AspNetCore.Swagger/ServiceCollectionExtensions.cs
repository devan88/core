using Core.Hosting.AspNetCore.Swagger;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace Microsoft.Extensions.DependencyInjection
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
                logger.LogInformation("TryAddSwaggerGen {@options}", swaggerOptions);
                services.AddSwaggerGen(c =>
                {
                    var apiOptions = swaggerOptions.SwaggerDoc.OpenApiOptions;
                    c.SwaggerDoc(swaggerOptions.SwaggerDoc.Name, new OpenApiInfo { Title = apiOptions.Title, Version = apiOptions.Version });
                });
            }
            catch (Exception ex)
            {
                logger.LogError("Error adding SwaggerGen: {@ex}", ex);
            }
            return services;
        }
    }
}
