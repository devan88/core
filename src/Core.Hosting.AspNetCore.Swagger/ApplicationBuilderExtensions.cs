using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Hosting.AspNetCore.Swagger
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwagger(
            this IApplicationBuilder app,
            IServiceProvider serviceProvider)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                SwaggerOptions swaggerOptions = serviceProvider.GetRequiredService<SwaggerOptions>();
                c.SwaggerEndpoint(swaggerOptions.SwaggerEndpoint.Url, swaggerOptions.SwaggerEndpoint.Name);
            });
            return app;
        }
    }
}
