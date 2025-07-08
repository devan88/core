using Microsoft.AspNetCore.Builder;

namespace Core.Logging.AspNetCore
{
    /// <summary>
    /// Provides extension methods for the IApplicationBuilder interface.
    /// </summary>
    public static class ApplicationBuilderExtensions
    {
        /// <summary>
        /// Adds the <see cref="CorrelationIdMiddleware" /> to the application's pipeline.
        /// </summary>
        /// <param name="app">The IApplicationBuilder instance to add the middleware to.</param>
        /// <returns>The IApplicationBuilder instance with the CorrelationIdMiddleware added.</returns>
        public static IApplicationBuilder UseCorrelationIdMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CorrelationIdMiddleware>();
        }
    }
}
