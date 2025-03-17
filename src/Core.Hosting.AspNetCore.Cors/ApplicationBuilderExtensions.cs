using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Core.Hosting.AspNetCore.Cors
{
    /// <summary>
    /// Provides extension methods for the IApplicationBuilder interface.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Enables CORS for the application using <see cref="IHostEnvironment.EnvironmentName"/>.
        /// </summary>
        /// <param name="app">The IApplicationBuilder instance to extend.</param>
        /// <param name="environment">The IHostEnvironment instance containing environment information.</param>
        /// <returns>The IApplicationBuilder instance with CORS enabled.</returns>
        public static IApplicationBuilder UseEnvironmentCors(this IApplicationBuilder app, IHostEnvironment environment)
        {
            app.UseCors(environment.EnvironmentName);

            return app;
        }
    }
}
