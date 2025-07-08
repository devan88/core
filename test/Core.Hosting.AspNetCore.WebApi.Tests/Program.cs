using Core.HttpClient.Authorization.Extensions;
using Core.HttpClient.Extensions;
using Core.Logging;
using Core.Logging.AspNetCore;
using Core.Logging.Serilog;
using Core.OpenTelemetry.AspNetCore;
using Microsoft.AspNetCore.Mvc.Formatters;
using OpenTelemetry.Exporter;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;

namespace Core.Hosting.AspNetCore.WebApi.Tests
{
    public class Program
    {
        protected Program() { }

        public static async Task Main(string[] args)
        {
            using CancellationTokenSource cts = new();
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            WebApiStartup<Program> startup = new(builder, builder.Host, loggingBuilder =>
            {
                loggingBuilder
                .ConfigureSerilog(builder.Configuration)
                .ConfigureSerilog(config =>
                {
                    config.WithLogstash(options =>
                    {
                        options.Url = "http://localhost:5044";
                    });
                });
            })
            {
                ConfigureLogging = (config, hostBuilder) =>
                {
                    hostBuilder.UseSerilog();
                },
                ConfigureServices = (services) =>
                {
                    //services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
                    services.AddAspNetCoreOpenTelemetry(
                    resourceBuilder =>
                    {
                        resourceBuilder.AddService("WebApi.Tests");
                    },
                    tracerProviderBuilder =>
                    {
                        tracerProviderBuilder.AddJaegerExporter(options =>
                        {
                            options.AgentHost = "localhost"; // Jaeger agent host
                            options.AgentPort = 6831; // Jaeger agent port
                        });
                    },
                    metricsProviderBuilder =>
                    {
                        metricsProviderBuilder.AddOtlpExporter(options =>
                        {
                            options.Endpoint = new Uri("http://localhost:4317"); // OTLP endpoint
                            options.Protocol = OtlpExportProtocol.Grpc;
                        });
                    });
                    services.AddCorrelationId();
                    services.AddOAuthTokenProviderFactory();
                    services
                    .AddStandardBaseHttpClient<HttpService>(builder.Configuration.GetSection("HttpClients:0"))
                    .WithBearerTokenAuthHandler(builder.Configuration.GetSection("HttpClients:0:Authorization"))
                    .AddHttpMessageHandler<CorrelationIdHandler>();
                    services
                    .AddStandardBaseHttpClient<OtherService>(builder.Configuration.GetSection("HttpClients:1"))
                    .WithWindowsAuthHandler(builder.Configuration.GetSection("HttpClients:1:Authorization"));
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
