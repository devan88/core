using Microsoft.Extensions.Configuration;
using Serilog;

namespace Microsoft.Extensions.Hosting
{
    public static class HostBuilderExtensions
    {

        public static IHostBuilder ConfigureSerilog(this IHostBuilder hostBuilder, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
               .ReadFrom.Configuration(configuration)
               .CreateLogger();

            return hostBuilder.UseSerilog();
        }
    }
}
