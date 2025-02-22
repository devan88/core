using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Core.HttpClient.Authorization.Extensions
{
    /// <summary>
    /// Provides extension methods for <see cref="IServiceCollection"/> to register OAuth token provider services.
    /// </summary>
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Adds the OAuth token provider factory to the specified <see cref="IServiceCollection"/>.
        /// This includes registration of an http client for AWS and the token provider factory as a singleton.
        /// </summary>
        /// <param name="services">The service collection to add the services to.</param>
        /// <returns>The updated <see cref="IServiceCollection"/> instance.</returns>
        public static IServiceCollection AddOAuthTokenProviderFactory(this IServiceCollection services)
        {
            services.AddHttpClient(Constant.AwsHttpClient);
            services.AddSingleton<IOAuthTokenProviderFactory, DefaultOAuthTokenProviderFactory>();
            return services;
        }
    }
}
