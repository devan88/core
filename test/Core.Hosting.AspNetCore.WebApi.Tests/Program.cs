namespace Core.Hosting.AspNetCore.WebApi.Tests
{
    public class Program
    {
        protected Program() { }

        public static async Task Main(string[] args)
        {
            CancellationTokenSource cts = new();
            var builder = WebApplication.CreateBuilder(args);
            var startup = new WebApiStartup<Program>(builder, builder.Host, builder =>
            {
                builder.AddSerilogDiagnostics();
            })
            {
                ConfigureLogging = (config, builder) =>
                {
                    builder.ConfigureSerilog(config);
                }
            };
            startup.Build();
            var app = builder.Build();
            startup.Configure(app, app, app.Environment);
            await app.RunWithLoggerAsync(cts.Token);
        }
    }
}
