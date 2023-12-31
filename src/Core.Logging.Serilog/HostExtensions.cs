using Serilog;

namespace Microsoft.Extensions.Hosting
{
    public static class HostExtensions
    {
        public static async Task RunWithLoggerAsync(this IHost host, CancellationToken cancellationToken = default)
        {
            try
            {
                Log.Information("App is starting.");
                await host.RunAsync(cancellationToken);
                Log.Information("App has successfully stopped.");
            }
            catch (Exception ex)
            {
                Log.Fatal("App has encountered an error {@ex}", ex);
            }
            finally
            {
                Log.Information("Flushing all logs.");
                Log.CloseAndFlush();
            }
        }
    }
}
