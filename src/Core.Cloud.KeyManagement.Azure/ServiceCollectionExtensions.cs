using Core.Cloud.Configurations.Azure.KeyVault;
using Core.Extensions.Azure;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Core.Cloud.KeyManagement.Azure
{
    /// <summary>
    /// Contains extension methods for the IServiceCollection to configure Azure clients and key management services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds and configures the Azure Secret Client(s) based on the Azure Key Vault configuration.
        /// </summary>
        /// <param name="services">The service collection to which the Azure Secret Client(s) will be added.</param>
        /// <param name="configuration">The application configuration used to extract Azure Key Vault settings.</param>
        /// <param name="environment">The hosting environment, which may be used to adjust client credentials.</param>
        /// <param name="logger">The logger instance used to log informational or warning messages.</param>
        /// <param name="credentials">
        /// Optional action that configures CredentialOptions for the Azure clients.
        /// If not specified, default credentials will be used.
        /// </param>
        /// <returns>The original service collection with the Azure Secret Client(s) added.</returns>
        public static IServiceCollection AddAzureSecretClient(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment,
            ILogger logger,
            Action<CredentialOptions> credentials = null)
        {
            IEnumerable<AzureKeyVault> configKeyVaults = configuration.GetAzureKeyVaults();
            if (configKeyVaults.Any())
            {
                services.AddAzureClients(builder =>
                {
                    for (int i = 0; i < configKeyVaults.Count(); i++)
                    {
                        AzureKeyVault config = configKeyVaults.ElementAt(i);
                        builder
                        .AddSecretClient(config.Uri())
                        .WithName(i == 0 ? "Default" : config.Name)
                        .WithCredential(environment, credentials)
                        .ConfigureOptions(configuration.AzureKeyVaultConfiguration());
                    }

                });
            }
            else
            {
                logger.LogWarning("Azure Key Vault configuration was not found");
            }
            return services;
        }

        /// <summary>
        /// Registers the Azure Key Management services in the dependency injection container.
        /// </summary>
        /// <param name="services">The service collection to which the Azure Key Management services will be added.</param>
        /// <returns>The original service collection with added key management services.</returns>
        public static IServiceCollection AddAzureKeyManagementServices(this IServiceCollection services)
        {
            services.AddScoped<IKeyManagementRepository, AzureKeyManagementRepository>();
            return services;
        }
    }
}
