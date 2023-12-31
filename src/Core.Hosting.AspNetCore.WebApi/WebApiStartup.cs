using Core.Extensions.Azure;
using Core.Hosting.AspNetCore.Swagger;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Hosting.AspNetCore.WebApi
{
    public sealed class WebApiStartup<T> : BaseStartup<T>
        where T : class
    {
        public Action<CredentialOptions>? ConfigureSecretCredential { get; set; }
        public Action<CredentialOptions>? ConfigureStorageCredential { get; set; }
        public Action<SwaggerOptions>? ConfigureSwagger { get; set; }
        public Action<MvcOptions>? ConfigureMvc { get; set; }

        private readonly SwaggerOptions _swaggerOptions;

        public WebApiStartup(
            IHostApplicationBuilder appBuilder,
            IHostBuilder hostBuilder,
            Action<ILoggingBuilder>? configureDiagnostics = null)
            : base(appBuilder, hostBuilder, configureDiagnostics)
        {
            _swaggerOptions = Configuration.GetSection(nameof(SwaggerOptions)).Get<SwaggerOptions>() ?? new SwaggerOptions();
        }

        public void Configure(
            IApplicationBuilder app,
            IEndpointRouteBuilder endpoints,
            IHostEnvironment env)
        {
            Configure(env);
            if (env.IsDevelopment() || env.IsLocalDevelopment())
            {
                app.UseSwagger(_swaggerOptions);
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
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
            ConfigureSwagger?.Invoke(_swaggerOptions);
            AppBuilder.Services.AddApiControllers(ConfigureMvc);
            AppBuilder.Services.TryAddSwaggerGen(_swaggerOptions, Logger);
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
