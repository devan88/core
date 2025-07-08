using Microsoft.Extensions.Configuration;
using OpenTelemetry.Exporter;

namespace Core.OpenTelemetry
{
    /// <summary>
    /// Provides extension methods for the IConfiguration interface.
    /// </summary>

    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Retrieves an action to configure <see cref="OtlpExporterOptions"/> from the specified configuration.
        /// This extension method allows for easy binding of configuration settings to the
        /// <see cref="OtlpExporterOptions"/>, facilitating the setup of the OTLP exporter.
        /// </summary>
        /// <param name="configuration">The <see cref="IConfiguration"/> instance containing the settings for the OTLP exporter.</param>
        /// <returns>An action that binds the <see cref="OtlpExporterOptions"/> to the configuration section specified for OTLP.</returns>
        public static Action<OtlpExporterOptions> GetOtlpExporterOptions(this IConfiguration configuration)
        {
            return options => configuration
            .GetSection(Constant.OtlpSection)
            .Bind(options);
        }
    }
}
