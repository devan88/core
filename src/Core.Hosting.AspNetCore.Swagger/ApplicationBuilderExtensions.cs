using Core.Hosting.AspNetCore.Swagger;
using Microsoft.AspNetCore.Builder;

namespace Core.Hosting.AspNetCore.Swagger
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
