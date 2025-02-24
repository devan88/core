using Core.HttpClient.Authorization.Extensions;
using Core.HttpClient.Extensions;
using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Core.Hosting.AspNetCore.WebApi.Tests
{
    public class Program
    {
        protected Program() { }

        public static async Task Main(string[] args)
        {
            using CancellationTokenSource cts = new();
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            WebApiStartup<Program> startup = new(builder, builder.Host, builder =>
            {
                builder.AddSerilogDiagnostics();
            })
            {
                ConfigureLogging = (config, builder) =>
                {
                    builder.ConfigureSerilog(config);
                },
                ConfigureServices = (services) =>
                {
                    services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
                    services.AddOAuthTokenProviderFactory();
                    services
                    .AddStandardBaseHttpClient<HttpService>(builder.Configuration, "HttpClients:0")
                    .WithWindowsAuthHandler(builder.Configuration, "HttpClients:0:Authorization");
                    services
                    .AddStandardBaseHttpClient<OtherService>(builder.Configuration, "HttpClients:1")
                    .WithWindowsAuthHandler(builder.Configuration, "HttpClients:1:Authorization");
                },
                ConfigureMvc = options =>
                {
                    // Add XML formatter
                    options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
                    options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
                }
            };
            startup.Build();
            WebApplication app = builder.Build();
            startup.Configure(app, app, app.Environment);
            await app.RunWithLoggerAsync(cts.Token);
        }
    }
}
