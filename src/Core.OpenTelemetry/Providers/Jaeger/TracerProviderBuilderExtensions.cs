using Microsoft.Extensions.Configuration;
using OpenTelemetry.Exporter;
using OpenTelemetry.Trace;

namespace Core.OpenTelemetry.Providers.Jaeger
{
    /// <summary>
    /// Provides extension methods for the TracerProviderBuilder abstract class.
    /// </summary>
    public static class TracerProviderBuilderExtensions
    {
        /// <summary>
        /// Adds a Jaeger exporter to the <see cref="TracerProviderBuilder"/> using the specified configuration.
        /// This method extends the functionality of the <see cref="TracerProviderBuilder"/> to include Jaeger
        /// as a tracing exporter, allowing for distributed tracing capabilities.
        /// </summary>
        /// <param name="tracerProviderBuilder">The <see cref="TracerProviderBuilder"/> instance to which the Jaeger exporter will be added.</param>
        /// <param name="configuration">The configuration object that contains settings for the Jaeger exporter.</param>
        /// <returns>The updated <see cref="TracerProviderBuilder"/> instance with the Jaeger exporter configured.</returns>
        public static TracerProviderBuilder AddJaeger(
            this TracerProviderBuilder tracerProviderBuilder,
            IConfiguration configuration)
        {
            Action<JaegerExporterOptions> configureOptions = options => configuration
                .GetSection(Constant.JaegerSection)
                .Bind(options);

            return tracerProviderBuilder.AddJaegerExporter(configureOptions);
        }

    }
}
