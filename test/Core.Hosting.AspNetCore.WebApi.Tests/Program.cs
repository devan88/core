using System.Text.Json;
using Core.Caching;
using Core.HttpClient.Authorization.Extensions;
using Core.HttpClient.Extensions;
using Core.HttpClient.Formatters;
using Core.Logging;
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
            await HostExtensions.RunWithLoggerAsync<Program>(args,
                loggingBuilder: config =>
                {
                    config.WithLogstash(options =>
                    {
                        options.Url = "http://localhost:5044";
                    });
                },
                hostBuilder: () =>
                {
                    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
                    WebApiStartup<Program> startup = new(builder, builder.Host)
                    {
                        ConfigureLogging = (hostBuilder, loggingBuilder) =>
                        {
                            hostBuilder.UseSerilog();
                        },
                        ConfigureOpenApiOptions = (options) =>
                        {
                            options.Title = "WebApi.Tests";
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

                            string name = builder.Configuration.GetSection("HttpClients:0:Authorization:ClientId").Value!;

                            services
                            .AddAzureAuthTokenProvider(builder.Configuration.GetSection("HttpClients:0:Authorization"), name);
                            //.AddAwsAuthTokenProvider(builder.Configuration.GetSection("HttpClients:1:Authorization"), name);
                            //.AddOAuthTokenProviderFactory();

                            services.AddCacheServices(builder.Configuration.GetSection("Cache:Redis"));

                            services
                            .AddStandardBaseHttpClient<HttpService>(builder.Configuration.GetSection("HttpClients:0"))
                            .AddHttpContentFormatter<JsonHttpContentFormatter, JsonSerializerOptions>(options =>
                            {
                                options.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.SnakeCaseLower;
                            })
                            .AddHttpContentFormatter<XmlHttpContentFormatter>()
                            .HttpClientBuilder
                            //.WithStandardResilienceHandler()
                            //.WithStandardHedgingHandler()
                            //.WithBearerTokenAuthHandler(builder.Configuration.GetSection("HttpClients:0:Authorization:ClientId").Value!)
                            .WithBearerTokenAuthHandler(name)
                            .AddHttpMessageHandler<CorrelationIdHandler>();

                            services
                            .AddStandardBaseHttpClient<OtherService>(builder.Configuration.GetSection("HttpClients:1"))
                            .HttpClientBuilder
                            .WithWindowsAuthHandler(builder.Configuration.GetSection("HttpClients:1:Authorization"));

                            //services.AddHttpClientJsonFormatter(options =>
                            //{
                            //    options.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
                            //});
                            //services.AddHttpClientJsonFormatter(options =>
                            //{
                            //    options.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.KebabCaseUpper;
                            //});
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
                    return app;
                });
        }

        //public static async Task Main(string[] args)
        //{
        //    using CancellationTokenSource cts = new();
        //    WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        //    WebApiStartup<Program> startup = new(builder, builder.Host)
        //    {
        //        ConfigureOpenApiOptions = (options) =>
        //        {
        //            options.Title = "WebApi.Tests";
        //        },
        //        ConfigureLogging = (hostBuilder, loggingBuilder) =>
        //        {
        //            hostBuilder.ConfigureSerilog(builder.Configuration, config =>
        //            {
        //                config.WithDefaultConfiguration();
        //                config.WithLogstash(options =>
        //                {
        //                    options.Url = "http://localhost:5044";
        //                });
        //            });
        //        },
        //        ConfigureServices = (services) =>
        //        {
        //            //services.AddAuthentication(NegotiateDefaults.AuthenticationScheme).AddNegotiate();
        //            //services.AddAspNetCoreOpenTelemetry(
        //            //resourceBuilder =>
        //            //{
        //            //    resourceBuilder.AddService("WebApi.Tests");
        //            //},
        //            //tracerProviderBuilder =>
        //            //{
        //            //    tracerProviderBuilder.AddJaegerExporter(options =>
        //            //    {
        //            //        options.AgentHost = "localhost"; // Jaeger agent host
        //            //        options.AgentPort = 6831; // Jaeger agent port
        //            //    });
        //            //},
        //            //metricsProviderBuilder =>
        //            //{
        //            //    metricsProviderBuilder.AddOtlpExporter(options =>
        //            //    {
        //            //        options.Endpoint = new Uri("http://localhost:4317"); // OTLP endpoint
        //            //        options.Protocol = OtlpExportProtocol.Grpc;
        //            //    });
        //            //});

        //            string name = builder.Configuration.GetSection("HttpClients:0:Authorization:ClientId").Value!;

        //            services
        //            .AddAzureAuthTokenProvider(builder.Configuration.GetSection("HttpClients:0:Authorization"), name);
        //            //.AddAwsAuthTokenProvider(builder.Configuration.GetSection("HttpClients:1:Authorization"), name);
        //            //.AddOAuthTokenProviderFactory();

        //            services.AddCacheServices(builder.Configuration.GetSection("Cache:Redis"));

        //            services
        //            .AddStandardBaseHttpClient<HttpService>(builder.Configuration.GetSection("HttpClients:0"))
        //            //.WithBearerTokenAuthHandler(builder.Configuration.GetSection("HttpClients:0:Authorization:ClientId").Value!)
        //            .WithBearerTokenAuthHandler(name)
        //            .AddHttpMessageHandler<CorrelationIdHandler>();

        //            services
        //            .AddStandardBaseHttpClient<OtherService>(builder.Configuration.GetSection("HttpClients:1"))
        //            .WithWindowsAuthHandler(builder.Configuration.GetSection("HttpClients:1:Authorization"));
        //        },
        //        ConfigureMvc = options =>
        //        {
        //            // Add XML formatter
        //            options.InputFormatters.Add(new XmlSerializerInputFormatter(options));
        //            options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
        //        }
        //    };
        //    startup.Build();
        //    WebApplication app = builder.Build();
        //    startup.Configure(app, app, app.Environment);
        //    await app.RunWithLoggerAsync(cts.Token);
        //}
    }
}
