using Azure.Core;
using Core.Extensions.Azure;
using Microsoft.Extensions.Hosting;

namespace Microsoft.Extensions.Azure
{
    public static class AzureClientFactoryBuilderExtensions
    {
        public static AzureClientFactoryBuilder UseCredential(
            this AzureClientFactoryBuilder builder,
            IHostEnvironment environment,
            Action<CredentialOptions> credentials = null)
        {
            var options = new CredentialOptions();
            credentials?.Invoke(options);
            return builder.UseCredential(environment, options);
        }

        public static AzureClientFactoryBuilder UseCredential(
            this AzureClientFactoryBuilder builder,
            IHostEnvironment environment,
            CredentialOptions options)
        {
            return builder.UseCredential(environment,
                options.LocalDevelopment,
                options.Development,
                options.Staging,
                options.Production);
        }

        private static AzureClientFactoryBuilder UseCredential(
            this AzureClientFactoryBuilder builder,
            IHostEnvironment environment,
            Func<IServiceProvider, TokenCredential> localDevelopmentCrendentials,
            Func<IServiceProvider, TokenCredential> developmentCrendentials,
            Func<IServiceProvider, TokenCredential> stagingCrendentials,
            Func<IServiceProvider, TokenCredential> productionCrendentials)
        {
            if (environment.IsLocalDevelopment())
            {
                builder.UseCredential(localDevelopmentCrendentials);
            }
            else if (environment.IsDevelopment())
            {
                builder.UseCredential(developmentCrendentials);
            }
            else if (environment.IsStaging())
            {
                builder.UseCredential(stagingCrendentials);
            }
            else if (environment.IsProduction())
            {
                builder.UseCredential(productionCrendentials);
            }
            return builder;
        }
    }
}
