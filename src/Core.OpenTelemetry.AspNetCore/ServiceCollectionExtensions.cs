using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Core.OpenTelemetry.AspNetCore
{
    /// <summary>
    /// Provides extension methods for the IServiceCollection interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds OpenTelemetry services for ASP.NET Core to the specified <see cref="IServiceCollection"/>.
        /// This method configures resources, tracing, and metrics providers with
        /// ASP.NET Core instrumentation and optional custom configurations provided by the caller.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which OpenTelemetry services will be added.</param>
        /// <param name="resourceBuilder">An optional action to customize the <see cref="ResourceBuilder"/> configuration.</param>
        /// <param name="tracerProviderBuilder">An optional action to customize the <see cref="TracerProviderBuilder"/> configuration.</param>
        /// <param name="metricsProviderBuilder">An optional action to customize the <see cref="MeterProviderBuilder"/> configuration.</param>
        /// <returns>An <see cref="IOpenTelemetryBuilder"/> instance for further configuration.</returns>
        public static IOpenTelemetryBuilder AddAspNetCoreOpenTelemetry(
            this IServiceCollection services,
            Action<ResourceBuilder>? resourceBuilder = null,
            Action<TracerProviderBuilder>? tracerProviderBuilder = null,
            Action<MeterProviderBuilder>? metricsProviderBuilder = null)
        {
            return services.AddCoreOpenTelemetry(
                resourceBuilder: resourceBuilder,
                tracerProviderBuilder: builder =>
                {
                    builder.AddAspNetCoreInstrumentation(); // Instrument ASP.NET Core
                    tracerProviderBuilder?.Invoke(builder);
                },
                metricsProviderBuilder: builder =>
                {
                    builder.AddAspNetCoreInstrumentation(); // Instrument ASP.NET Core
                    metricsProviderBuilder?.Invoke(builder);
                });
        }
    }
}
