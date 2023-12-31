using Core.Extensions.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Hosting
{
    public abstract class BaseStartup<T>
        where T : class
    {
        public Action<IServiceCollection>? ConfigureServices { get; set; }
        public Action<IConfiguration, IHostBuilder>? ConfigureLogging { get; set; }
        public Action<IConfigurationBuilder>? ConfigureConfiguration { get; set; }
        public Func<IConfigurationBuilder, string>? ConfigureKeyPerFilePath { get; set; }

        protected readonly IHostApplicationBuilder AppBuilder;
        protected readonly IHostBuilder HostBuilder;
        protected readonly ILoggingBuilder LoggingBuilder;
        protected readonly IConfigurationManager Configuration;

        protected virtual ILogger Logger => AppBuilder.GetDiagnosticsLogger();
        protected virtual bool ShouldValidate =>
            AppBuilder.Environment.IsLocalDevelopment() ||
            AppBuilder.Environment.IsDevelopment();

        protected BaseStartup(
            IHostApplicationBuilder appBuilder,
            IHostBuilder hostBuilder,
            Action<ILoggingBuilder>? configureDiagnostics)
        {
            AppBuilder = appBuilder;
            HostBuilder = hostBuilder;
            LoggingBuilder = AppBuilder.Logging;
            Configuration = AppBuilder.Configuration;
            ConfigureDiagnosticLogging(configureDiagnostics);
            ConfigureDefaultBuilder();
        }

        public virtual void Build()
        {
            ConfigureConfiguration?.Invoke(Configuration);
            ConfigureLogging?.Invoke(Configuration, HostBuilder);
            ConfigureServices?.Invoke(AppBuilder.Services);
        }

        public virtual void Configure(IHostEnvironment env)
        {

        }

        protected void ConfigureDiagnosticLogging(Action<ILoggingBuilder>? configureDiagnostics)
        {
            LoggingBuilder.AddDiagnostics();
            configureDiagnostics?.Invoke(LoggingBuilder);
            AppBuilder.AddDiagnosticsLogger<T>(LoggingBuilder);
        }

        private void ConfigureDefaultBuilder()
        {
            ConfigureValidation();
            ConfigureConfiguration += (builder) =>
            {
                builder.AddUserSecrets<T>(AppBuilder.Environment);
                builder.TryAddKeyPerFile(Logger, ConfigureKeyPerFilePath?.Invoke(Configuration) ?? "/mnt/secrets/");
            };
            
        }

        private void ConfigureValidation()
        {
            HostBuilder.UseDefaultServiceProvider(options =>
            {
                options.ValidateScopes = ShouldValidate;
                options.ValidateOnBuild = ShouldValidate;
            });
        }
    }
}
