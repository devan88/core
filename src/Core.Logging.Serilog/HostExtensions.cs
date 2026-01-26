using Core.Hosting;
using Microsoft.Extensions.Configuration;
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

        /// <summary>
        /// Runs the host with logging, including startup, shutdown, and error handling.
        /// </summary>
        /// <typeparam name="T">The type used for configuration building.</typeparam>
        /// <param name="args">The command-line arguments passed to the application.</param>
        /// <param name="hostBuilder">A function that creates and configures the host.</param>
        /// <param name="loggingBuilder">An optional action to configure the logger settings.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task RunWithLoggerAsync<T>(
            string[] args,
            Func<IHost> hostBuilder,
            Action<LoggerConfiguration>? loggingBuilder = null)
        {
            IConfiguration configuration = ConfigurationFactory.BuildConfiguration<T>(args);

            SerilogLoggerBuilder loggerBuilder = SerilogLoggerBuilder
                .CreateBuilder(configuration)
                .Configure(loggingBuilder ?? (_ => { }));
            loggerBuilder.Build();

            try
            {
                using CancellationTokenSource cts = new();
                IHost app = hostBuilder.Invoke();
                Log.Information("Application is starting.");
                await app.RunAsync(cts.Token);
                Log.Information("Application has gracefully stopped.");
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Application has encountered an error.");
            }
            finally
            {
                loggerBuilder.Shutdown();
            }
        }
    }
}
