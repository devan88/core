using Core.Hosting.AspNetCore.Swagger;

namespace Microsoft.AspNetCore.Builder
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwagger(
            this IApplicationBuilder app,
            SwaggerOptions swaggerOptions)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint(swaggerOptions.SwaggerEndpoint.Url, swaggerOptions.SwaggerEndpoint.Name);
            });
            return app;
        }
    }
}
