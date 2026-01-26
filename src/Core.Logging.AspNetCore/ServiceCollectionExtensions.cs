using Microsoft.Extensions.DependencyInjection;

namespace Core.Logging.AspNetCore
{
    /// <summary>
    /// Provides extension methods for the IServiceCollection interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds correlation ID services to the IServiceCollection.
        /// This includes adding IHttpContextAccessor, ICorrelationIdService, and CorrelationIdHandler.
        /// </summary>
        /// <param name="services">The IServiceCollection to add services to.</param>
        /// <returns>The IServiceCollection with correlation ID services added.</returns>
        public static IServiceCollection AddHttpCorrelationId(this IServiceCollection services)
        {
            return services
                .AddHttpContextAccessor()
                .AddTransient<ICorrelationIdService, HttpContextCorrelationIdService>()
                .AddTransient<CorrelationIdHandler>();
        }
    }
}
