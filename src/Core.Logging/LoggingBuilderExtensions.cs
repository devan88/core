using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Logging
{
    /// <summary>
    /// Provides extension methods for configuring <see cref="ILoggingBuilder"/>.
    /// </summary>
    public static class LoggingBuilderExtensions
    {
        /// <summary>
        /// Configures logging services using the specified configuration settings.
        /// </summary>
        /// <param name="builder">The logging builder to extend.</param>
        /// <param name="configuration">The configuration object containing logging settings.</param>
        /// <returns>The updated logging builder.</returns>
        public static ILoggingBuilder ConfigureLogging(this ILoggingBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddSingleton(sp =>
            {
                return LoggerFactory.Create(builder =>
                {
                    builder
                    .AddConfiguration(configuration.GetSection("Logging"))
                    .AddConsole();
                });
            });

            return builder;
        }
    }
}
