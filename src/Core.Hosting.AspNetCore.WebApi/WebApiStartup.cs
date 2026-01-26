using Core.Cloud.KeyManagement.Azure;
using Core.Extensions.Azure;
using Core.Extensions.Hosting;
using Core.Hosting.AspNetCore.Swagger;
using Core.Logging.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Core.Hosting.AspNetCore.WebApi
{
    public sealed class WebApiStartup<T> : BaseStartup<T>
        where T : class
    {
        public Action<CredentialOptions>? ConfigureSecretCredential { get; set; }
        public Action<CredentialOptions>? ConfigureStorageCredential { get; set; }
        public Action<OpenApiOptions>? ConfigureOpenApiOptions { get; set; }
        public Action<OpenApiInfo>? ConfigureOpenApiInfo { get; set; }
        public Action<MvcOptions>? ConfigureMvc { get; set; }

        public WebApiStartup(
            IHostApplicationBuilder appBuilder,
            IHostBuilder hostBuilder)
            : base(appBuilder, hostBuilder)
        {

        }

        public void Configure(
            IApplicationBuilder app,
            IEndpointRouteBuilder endpoints,
            IHostEnvironment env)
        {
            Configure(env);
            if (env.IsDevelopment() || env.IsLocalDevelopment())
            {
                app.UseVersioningSwagger(endpoints);
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.UseCorrelationIdMiddleware();
            endpoints.MapControllers();
        }


        public override void Build()
        {
            base.Build();
            ConfigureApiServices();
            ConfigureAzureServices();
        }

        private void ConfigureApiServices()
        {
            AppBuilder.Services.AddHttpCorrelationId();
            AppBuilder.Services.AddCoreApiVersioning();
            AppBuilder.Services.AddApiControllers(ConfigureMvc);
            AppBuilder.Services.AddVersioningSwaggerGen(ConfigureOpenApiOptions, ConfigureOpenApiInfo);
        }

        private void ConfigureAzureServices()
        {
            AppBuilder.Services.AddAzureSecretClient(
                Configuration,
                AppBuilder.Environment,
                Logger,
                ConfigureSecretCredential);
            AppBuilder.Services.AddAzureStorageClient(
                Configuration,
                AppBuilder.Environment,
                Logger,
                ConfigureStorageCredential);
        }
    }
}
