using Azure.Core;
using Azure.Core.Extensions;
using Microsoft.Extensions.Azure;
using Microsoft.Extensions.Hosting;

namespace Core.Extensions.Azure
{
    public static class AzureClientBuilderExtensions
    {
        public static IAzureClientBuilder<TClient, TOptions> WithCredential<TClient, TOptions>(
            this IAzureClientBuilder<TClient, TOptions> builder,
            IHostEnvironment environment,
            Action<CredentialOptions> credentials = null)
            where TOptions : class
        {
            var options = new CredentialOptions();
            credentials?.Invoke(options);
            return builder.WithCredential(environment, options);
        }

        public static IAzureClientBuilder<TClient, TOptions> WithCredential<TClient, TOptions>(
            this IAzureClientBuilder<TClient, TOptions> builder,
            IHostEnvironment environment,
            CredentialOptions options)
            where TOptions : class
        {
            return builder.WithCredential(environment,
                options.LocalDevelopment,
                options.Development,
                options.Staging,
                options.Production);
        }

        private static IAzureClientBuilder<TClient, TOptions> WithCredential<TClient, TOptions>(
            this IAzureClientBuilder<TClient, TOptions> builder,
            IHostEnvironment environment,
            Func<IServiceProvider, TokenCredential> localDevelopmentCrendentials,
            Func<IServiceProvider, TokenCredential> developmentCrendentials,
            Func<IServiceProvider, TokenCredential> stagingCrendentials,
            Func<IServiceProvider, TokenCredential> productionCrendentials)
            where TOptions : class
        {
            if (environment.IsLocalDevelopment())
            {
                builder.WithCredential(localDevelopmentCrendentials);
            }
            else if (environment.IsDevelopment())
            {
                builder.WithCredential(developmentCrendentials);
            }
            else if (environment.IsStaging())
            {
                builder.WithCredential(stagingCrendentials);
            }
            else if (environment.IsProduction())
            {
                builder.WithCredential(productionCrendentials);
            }
            return builder;
        }
    }
}
