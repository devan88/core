using Core.HttpClient.Authorization.Aws;
using Core.HttpClient.Authorization.Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Core.HttpClient.Authorization
{
    internal sealed class DefaultOAuthTokenProviderFactory : IOAuthTokenProviderFactory
    {
        private readonly ILogger<DefaultOAuthTokenProviderFactory> _logger;
        private readonly IServiceProvider _services;

        public DefaultOAuthTokenProviderFactory(
            ILogger<DefaultOAuthTokenProviderFactory> logger,
            IServiceProvider services)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _services = services ?? throw new ArgumentNullException(nameof(services));
        }

        public IOAuthTokenProviderService CreateOAuthTokenProviderService(IConfiguration configuration, CloudProvider cloudProvider)
        {
            return cloudProvider switch
            {
                CloudProvider.Azure => CreateAzureOAuthTokenProvider(configuration),
                CloudProvider.Aws => CreateAwsOAuthTokenProvider(configuration),
                _ => throw new ArgumentNullException(nameof(cloudProvider), "Cloud type is not supported"),
            };
        }

        private AwsOAuthTokenProviderService CreateAwsOAuthTokenProvider(IConfiguration configuration)
        {
            _logger.LogInformation("Creating AwsOAuthTokenProvider");

            IOptions<AwsAuthConfiguration> options = GetOptions<AwsAuthConfiguration>(configuration);

            System.Net.Http.HttpClient htppClient = _services
                .GetRequiredService<IHttpClientFactory>()
                .CreateClient(Constant.AwsHttpClient);

            ILogger<AwsOAuthTokenProviderService> logger = _services
                .GetRequiredService<ILogger<AwsOAuthTokenProviderService>>();

            return new AwsOAuthTokenProviderService(logger, options, htppClient);
        }

        private AzureOAuthTokenProviderService CreateAzureOAuthTokenProvider(IConfiguration configuration)
        {
            _logger.LogInformation("Creating AzureOAuthTokenProvider");

            IOptions<AzureAuthConfiguration> options = GetOptions<AzureAuthConfiguration>(configuration);

            ILogger<AzureOAuthTokenProviderService> logger = _services
                .GetRequiredService<ILogger<AzureOAuthTokenProviderService>>();

            return new AzureOAuthTokenProviderService(logger, options);
        }

        private static IOptions<TConfig> GetOptions<TConfig>(IConfiguration configuration)
            where TConfig : class
        {
            TConfig azureAuthConfiguration = configuration.Get<TConfig>()
                        ?? throw new ArgumentNullException(nameof(configuration), "Authorization configuration cannot be found");

            return Options.Create(azureAuthConfiguration);
        }
    }
}
