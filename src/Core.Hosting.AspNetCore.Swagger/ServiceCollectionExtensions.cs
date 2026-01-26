using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Core.Hosting.AspNetCore.Swagger
{
    /// <summary>
    /// Provides extensions for adding SwaggerGen to the IServiceCollection.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddVersioningSwaggerGen(
            this IServiceCollection services,
            Action<OpenApiOptions>? configureOpenApiOptions = null,
            Action<OpenApiInfo>? configureOpenApiInfoOptions = null,
            Action<SwaggerGenOptions>? configureSwaggerGenOptions = null)
        {
            IApiVersionDescriptionProvider provider = services
                .BuildServiceProvider()
                .GetRequiredService<IApiVersionDescriptionProvider>();

            services.AddSwaggerGen(options =>
            {
                foreach (ApiVersionDescription description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerDoc(
                        description.GroupName,
                        ConfigureOpenApiInfo(description, configureOpenApiOptions, configureOpenApiInfoOptions));
                }
                options.AddBearerTokenAuthorization();
                configureSwaggerGenOptions?.Invoke(options);
            });

            return services;
        }

        private static OpenApiInfo ConfigureOpenApiInfo(
            ApiVersionDescription description,
            Action<OpenApiOptions>? configureOpenApiOptions = null,
            Action<OpenApiInfo>? configureOptions = null)
        {
            OpenApiOptions openApiOptions = new();
            configureOpenApiOptions?.Invoke(openApiOptions);

            var options = new OpenApiInfo()
            {
                Title = openApiOptions.Title,
                Version = description.ApiVersion.ToString(),
                Description = openApiOptions.Description
            };
            configureOptions?.Invoke(options);
            return options;
        }
    }
}
