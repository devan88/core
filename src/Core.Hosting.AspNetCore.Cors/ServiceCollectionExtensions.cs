using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Core.Hosting.AspNetCore.Cors
{
    /// <summary>
    /// Provides extension methods for IServiceCollection to add CORS configuration.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds default CORS configuration to the IServiceCollection
        /// with the policy name from <see cref="IHostEnvironment.EnvironmentName"/>.
        /// </summary>
        /// <param name="services">The IServiceCollection to add CORS configuration to.</param>
        /// <param name="environment">The IHostEnvironment to determine the environment name.</param>
        /// <param name="configuration">
        /// The IConfiguration to retrieve CORS options from section
        /// <see cref="Constant.CorsOptionsSection"/>.
        /// </param>
        /// <returns>The IServiceCollection with CORS configuration added.</returns>
        public static IServiceCollection AddEnvironmentCors(
            this IServiceCollection services,
            IHostEnvironment environment,
            IConfiguration configuration)
        {
            CorsOptions corsOptions = configuration
                .GetSection(Constant.CorsOptionsSection)
                .Get<CorsOptions>() ?? new();

            return services.AddCors(environment, corsOptions);
        }

        /// <summary>
        /// Adds CORS configuration to the IServiceCollection using a setup action
        /// with the policy name from <see cref="IHostEnvironment.EnvironmentName"/>.
        /// </summary>
        /// <param name="services">The IServiceCollection to add CORS configuration to.</param>
        /// <param name="environment">The IHostEnvironment to determine the environment name.</param>
        /// <param name="setupAction">The action to configure the <see cref="CorsOptions"/>.</param>
        /// <returns>The IServiceCollection with CORS configuration added.</returns>
        public static IServiceCollection AddCors(
            this IServiceCollection services,
            IHostEnvironment environment,
            Action<CorsOptions> setupAction)
        {
            CorsOptions corsOptions = new();
            setupAction(corsOptions);

            return services.AddCors(environment, corsOptions);
        }

        /// <summary>
        /// Adds CORS configuration to the IServiceCollection using the provided <see cref="CorsOptions"/>
        /// with the policy name from <see cref="IHostEnvironment.EnvironmentName"/>.
        /// </summary>
        /// <param name="services">The IServiceCollection to add CORS configuration to.</param>
        /// <param name="environment">The IHostEnvironment to determine the environment name.</param>
        /// <param name="corsOptions">The <see cref="CorsOptions"/> to use for CORS configuration.</param>
        /// <returns>The IServiceCollection with CORS configuration added.</returns>
        public static IServiceCollection AddCors(
            this IServiceCollection services,
            IHostEnvironment environment,
            CorsOptions corsOptions)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(environment.EnvironmentName, builder =>
                {
                    builder.WithOrigins(corsOptions.AllowedEndpoints)
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}
