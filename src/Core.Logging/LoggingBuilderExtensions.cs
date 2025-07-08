using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Logging
{
    public static class LoggingBuilderExtensions
    {
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
