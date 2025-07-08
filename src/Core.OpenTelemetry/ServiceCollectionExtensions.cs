using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Core.OpenTelemetry
{
    /// <summary>
    /// Provides extension methods for the IServiceCollection interface.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds OpenTelemetry services to the specified <see cref="IServiceCollection"/>.
        /// This method configures resources, tracing, and metrics providers
        /// with optional custom configurations provided by the caller.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to which OpenTelemetry services will be added.</param>
        /// <param name="resourceBuilder">An optional action to customize the <see cref="ResourceBuilder"/> configuration.</param>
        /// <param name="tracerProviderBuilder">An optional action to customize the <see cref="TracerProviderBuilder"/> configuration.</param>
        /// <param name="metricsProviderBuilder">An optional action to customize the <see cref="MeterProviderBuilder"/> configuration.</param>
        /// <returns>An <see cref="IOpenTelemetryBuilder"/> instance for further configuration.</returns>
        public static IOpenTelemetryBuilder AddCoreOpenTelemetry(
            this IServiceCollection services,
            Action<ResourceBuilder>? resourceBuilder = null,
            Action<TracerProviderBuilder>? tracerProviderBuilder = null,
            Action<MeterProviderBuilder>? metricsProviderBuilder = null)
        {
            return services
                .AddOpenTelemetry()
                .ConfigureResource(builder =>
                {
                    builder.ConfigureDefaultResource();
                    resourceBuilder?.Invoke(builder);
                })
                .WithTracing(builder =>
                {
                    builder.ConfigureDefaultTracing();
                    tracerProviderBuilder?.Invoke(builder);
                })
                .WithMetrics(builder =>
                {
                    builder.ConfigureDefaultMetrics();
                    metricsProviderBuilder?.Invoke(builder);
                });
        }

        /// <summary>
        /// Configures default resource settings for the <see cref="ResourceBuilder"/>.
        /// This method adds the service name based on the entry assembly.
        /// </summary>
        /// <param name="resourceBuilder">The <see cref="ResourceBuilder"/> instance to configure.</param>

        private static void ConfigureDefaultResource(this ResourceBuilder resourceBuilder)
        {
            resourceBuilder
                .AddService(Assembly.GetEntryAssembly()?.GetName().Name ?? string.Empty);
        }

        /// <summary>
        /// Configures default tracing settings for the <see cref="TracerProviderBuilder"/>.
        /// This method adds HTTP client instrumentation and console exporting for traces.
        /// </summary>
        /// <param name="tracerProviderBuilder">The <see cref="TracerProviderBuilder"/> instance to configure.</param>
        private static void ConfigureDefaultTracing(this TracerProviderBuilder tracerProviderBuilder)
        {
            tracerProviderBuilder
                .AddHttpClientInstrumentation() // Instrument HTTP client calls
                .AddConsoleExporter(); // Export traces to console
        }

        /// <summary>
        /// Configures default metrics settings for the <see cref="MeterProviderBuilder"/>.
        /// This method adds HTTP client instrumentation and console exporting for metrics.
        /// </summary>
        /// <param name="metricsProviderBuilder">The <see cref="MeterProviderBuilder"/> instance to configure.</param>
        private static void ConfigureDefaultMetrics(this MeterProviderBuilder metricsProviderBuilder)
        {
            metricsProviderBuilder
                .AddHttpClientInstrumentation() // Instrument HTTP for metrics
                .AddConsoleExporter(); // Export metrics to console
        }
    }
}
