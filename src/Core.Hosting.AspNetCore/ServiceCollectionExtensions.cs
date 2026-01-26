using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Core.Hosting.AspNetCore
{
    /// <summary>
    /// Provides extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds JWT authentication services to the service collection.
        /// </summary>
        /// <param name="services">The service collection to extend.</param>
        /// <param name="configuration">The configuration object to retrieve JWT options.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddJwtAuthorization(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            JwtAuthOptions jwtAuthOptions = configuration.Get<JwtAuthOptions>()!;
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = jwtAuthOptions.Authority;
                    options.RequireHttpsMetadata = jwtAuthOptions.RequiresHttpsMetadata;
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateIssuer = jwtAuthOptions.ValidateIssuer,
                        ValidIssuer = jwtAuthOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtAuthOptions.Audience,
                        ValidateLifetime = jwtAuthOptions.ValidateLifetime,
                        ValidateIssuerSigningKey = true
                    };
                });
            return services;
        }

        public static IServiceCollection AddHeaderAuthorization(
            this IServiceCollection services)
        {
            //services
            //    .AddAuthorizationBuilder()
            //    .AddPolicy("CustomHeaderPolicy", policy =>
            //    {
            //        policy.Requirements.Add(new HeaderRequirement("Twitch-Eventsub-Message-Signature", "lol"));
            //    });
            //services.AddAuthentication("AllowAll")
            //services.AddHttpContextAccessor();
            //services.AddAuthorization(options =>
            //{
            //    options.AddPolicy("CustomHeaderPolicy", policy =>
            //    {
            //        policy.Requirements.Add(new HeaderRequirement("Twitch-Eventsub-Message-Signature", "lol"));
            //        //policy.AddAuthenticationSchemes("AllowAll");
            //    });
            //});
            //services.AddSingleton<IAuthorizationHandler, HeaderRequirementHandler>();
            return services;
        }

        /// <summary>
        /// Adds API versioning and exploration services to the service collection.
        /// </summary>
        /// <param name="services">The service collection to extend.</param>
        /// <param name="configureApiVersioningOptions">An optional action to configure API versioning options.</param>
        /// <param name="configureApiExplorerOptions">An optional action to configure API explorer options.</param>
        /// <returns>The updated service collection.</returns>
        public static IServiceCollection AddCoreApiVersioning(
            this IServiceCollection services,
            Action<ApiVersioningOptions>? configureApiVersioningOptions = null,
            Action<ApiExplorerOptions>? configureApiExplorerOptions = null)
        {
            Action<ApiVersioningOptions> apiVersioningOptions =
                ConfigureApiVersioningOptions(configureApiVersioningOptions);

            Action<ApiExplorerOptions> apiExplorerOptions =
                ConfigureApiExplorerOptions(configureApiExplorerOptions);

            services
                .AddEndpointsApiExplorer()
                .AddApiVersioning(apiVersioningOptions)
                .AddMvc() // This is needed for controllers
                .AddApiExplorer(apiExplorerOptions);

            return services;
        }

        private static Action<ApiVersioningOptions> ConfigureApiVersioningOptions(
            Action<ApiVersioningOptions>? configureApiVersioningOptions = null)
        {
            return options =>
            {
                options.DefaultApiVersion = new ApiVersion(1);
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.ApiVersionReader = ApiVersionReader.Combine(
                        new UrlSegmentApiVersionReader(),
                        new HeaderApiVersionReader("X-Api-Version"));
                configureApiVersioningOptions?.Invoke(options);
            };
        }

        private static Action<ApiExplorerOptions> ConfigureApiExplorerOptions(
            Action<ApiExplorerOptions>? configureApiExplorerOptions = null)
        {
            return options =>
            {
                options.GroupNameFormat = "'v'V";
                options.SubstituteApiVersionInUrl = true;
                configureApiExplorerOptions?.Invoke(options);
            };
        }
    }
}
