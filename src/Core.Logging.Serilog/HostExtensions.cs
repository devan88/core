using Microsoft.Extensions.Hosting;
using Serilog;

namespace Core.Logging.Serilog
{
    /// <summary>
    /// Provides extensions for running the host with logging capabilities.
    /// </summary>
    public static class HostExtensions
    {
        /// <summary>
        /// Runs the host with logging, including startup, shutdown, and error handling.
        /// </summary>
        /// <param name="host">The host to run.</param>
        /// <param name="cancellationToken">The cancellation token to cancel the operation. Defaults to default if not specified.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
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
                Log.Fatal(ex, "App has encountered an error");
            }
            finally
            {
                Log.Information("Flushing all logs.");
                await Log.CloseAndFlushAsync();
            }
        }
    }
}
