using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Core.Logging.AspNetCore.Serilog
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
