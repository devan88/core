using Azure.Identity;
using Core.Azure.Identity;
using Core.Cloud.KeyManagement;
using Core.Cloud.Storage;
using Core.Cloud.Storage.Azure;
using Core.Extensions.Azure;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAzureStorageClient(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment,
            ILogger logger,
            Action<CredentialOptions> credentials = null)
        {
            var configAzureStorages = configuration.GetAzureStorages();
            if (configAzureStorages.Any())
            {
                services.AddAzureClients(builder =>
                {
                    for (int i = 0; i < configAzureStorages.Count(); i++)
                    {
                        var config = configAzureStorages.ElementAt(i);
                        builder
                        .AddBlobServiceClient(config.Uri())
                        .WithName(i == 0 ? "Default" : config.Name)
                        .WithCredential(environment, credentials)
                        .ConfigureOptions(configuration.AzureStorageConfiguration());
                        services.AddServicePrincipals(config.ServicePrincipalCredentials, config.Uri());
                    }

                });
            }
            else
            {
                logger.LogWarning("Azure Storage configuration was not found");
            }
            return services;
        }

        public static IServiceCollection AddAzureStorageServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAzureStorageOptions<AzureStorageConfiguration>(configuration);
            services.AddScoped<IStorageRepository, AzureStorageRepository>();
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            return services;
        }


        private static IServiceCollection AddServicePrincipals(
            this IServiceCollection services,
            IEnumerable<ServicePrincipalCredential> servicePrincipalCrendentials,
            Uri uri)
        {
            foreach (var spc in servicePrincipalCrendentials)
            {
                services.AddAzureClients(builder =>
                {
                    builder
                    .AddBlobServiceClient(uri)
                    .WithName(spc.Name)
                    .WithCredential(provider => GetClientSecretCredential(provider, spc));

                });
            }
            return services;

        }

        private static ClientSecretCredential GetClientSecretCredential(
            IServiceProvider provider,
            ServicePrincipalCredential spc)
        {
            using var scope = provider.CreateScope();
            var keyManagementRepository = scope.ServiceProvider.GetService<IKeyManagementRepository>();
            var secret = keyManagementRepository.GetSecretAsync(spc.ClientSecretKey, default).Result;
            return new ClientSecretCredential(spc.TenantId, spc.ClientId, secret);
        }
    }
}
