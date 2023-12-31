using Core.Cloud.KeyManagement;
using Core.Cloud.KeyManagement.Azure;
using Core.Extensions.Azure;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureSecretClient(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment,
            ILogger logger,
            Action<CredentialOptions> credentials = null)
        {
            var configKeyVaults = configuration.GetAzureKeyVaults();
            if (configKeyVaults.Any())
            {
                services.AddAzureClients(builder =>
                {
                    for (int i = 0; i < configKeyVaults.Count(); i++)
                    {
                        var config = configKeyVaults.ElementAt(i);
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

        public static IServiceCollection AddAzureKeyManagementServices(this IServiceCollection services)
        {
            services.AddScoped<IKeyManagementRepository, AzureKeyManagementRepository>();
            return services;
        }
    }
}
