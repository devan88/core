using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Core.Hosting.AspNetCore.Swagger
{
    /// <summary>
    /// Provides extension methods for configuring the application builder,
    /// specifically for setting up Swagger with API versioning.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Configures Swagger and Swagger UI for the application, supporting API versioning.
        /// </summary>
        /// <param name="app">The application builder to configure.</param>
        /// <param name="endpointRouteBuilder">The endpoint route builder used to describe API versions.</param>
        /// <returns>The updated application builder.</returns>
        public static IApplicationBuilder UseVersioningSwagger(
            this IApplicationBuilder app,
            IEndpointRouteBuilder endpointRouteBuilder)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (ApiVersionDescription description in endpointRouteBuilder.DescribeApiVersions())
                {
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"API {description.ApiVersion}");
                    options.RoutePrefix = "swagger";
                }

            });
            return app;
        }
    }
}
