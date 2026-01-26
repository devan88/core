using Core.Extensions.Configuration;
using Core.Extensions.Hosting;
using Core.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Hosting
{
    /// <summary>
    /// An abstract base class for configuring application startup,
    /// providing methods to set up services, logging, and configuration.
    /// </summary>
    /// <typeparam name="T">The type used to identify the user secrets and other configurations.</typeparam>
    public abstract class BaseStartup<T>
        where T : class
    {
        /// <summary>
        /// An optional action to configure services in the service collection.
        /// </summary>
        public Action<IServiceCollection>? ConfigureServices { get; set; }

        /// <summary>
        /// An optional action to configure logging for the host builder and logging builder.
        /// </summary>
        public Action<IHostBuilder, ILoggingBuilder>? ConfigureLogging { get; set; }

        /// <summary>
        /// An optional action to configure the configuration builder.
        /// </summary>
        public Action<IConfigurationBuilder>? ConfigureConfiguration { get; set; }

        /// <summary>
        /// An optional function to configure the key-per-file path.
        /// </summary>
        public Func<IConfigurationBuilder, string>? ConfigureKeyPerFilePath { get; set; }

        /// <summary>
        /// The application builder used to configure the application's request pipeline and services.
        /// </summary>
        protected readonly IHostApplicationBuilder AppBuilder;

        /// <summary>
        /// The host builder used to configure the application's host, including services and logging.
        /// </summary>
        protected readonly IHostBuilder HostBuilder;

        /// <summary>
        /// The logging builder used to configure logging services for the application.
        /// </summary>
        protected readonly ILoggingBuilder LoggingBuilder;

        /// <summary>
        /// The configuration manager used to access and manage application configuration settings.
        /// </summary>
        protected readonly IConfigurationManager Configuration;

        /// <summary>
        /// Gets the logger for diagnostics.
        /// </summary>
        protected virtual ILogger Logger => AppBuilder.GetDiagnosticsLogger();

        /// <summary>
        /// Determines whether to validate service scopes based on the environment.
        /// </summary>
        protected virtual bool ShouldValidate =>
            AppBuilder.Environment.IsLocalDevelopment() ||
            AppBuilder.Environment.IsDevelopment();

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseStartup{T}"/> class.
        /// </summary>
        /// <param name="appBuilder">The application builder used to configure the application.</param>
        /// <param name="hostBuilder">The host builder used to configure the host.</param>
        protected BaseStartup(
            IHostApplicationBuilder appBuilder,
            IHostBuilder hostBuilder)
        {
            AppBuilder = appBuilder;
            HostBuilder = hostBuilder;
            LoggingBuilder = AppBuilder.Logging;
            Configuration = AppBuilder.Configuration;
        }

        /// <summary>
        /// Builds the application by configuring logging, services, and configuration.
        /// </summary>
        public virtual void Build()
        {
            ConfigureAllLogging();
            ConfigureDefaultBuilder();
            ConfigureConfiguration?.Invoke(Configuration);
            ConfigureServices?.Invoke(AppBuilder.Services);
        }

        /// <summary>
        /// Configures the application based on the hosting environment.
        /// </summary>
        /// <param name="env">The hosting environment.</param>
        public virtual void Configure(IHostEnvironment env)
        {
            // Override this method in derived classes to provide environment-specific configuration.
        }

        /// <summary>
        /// Configures all logging for the application.
        /// </summary>
        protected void ConfigureAllLogging()
        {
            ConfigureLogging?.Invoke(HostBuilder, LoggingBuilder);
            AppBuilder.AddDiagnosticsLogger<T>(LoggingBuilder);
        }

        /// <summary>
        /// Configures the default builder settings for the application.
        /// </summary>
        private void ConfigureDefaultBuilder()
        {
            ConfigureValidation();
            ConfigureConfiguration += (builder) =>
            {
                builder.AddUserSecrets<T>(AppBuilder.Environment);
                builder.TryAddKeyPerFile(Logger, ConfigureKeyPerFilePath?.Invoke(Configuration) ?? "/mnt/secrets/");
            };

        }

        /// <summary>
        /// Configures validation settings for the service provider.
        /// </summary>
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
