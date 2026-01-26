using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace Core.Hosting.AspNetCore
{
    /// <summary>
    /// Provides extension methods for configuring endpoint routing in an ASP.NET Core application.
    /// </summary>
    public static class EndpointRouteBuilderExtensions
    {
        /// <summary>
        /// Maps a base route group for API endpoints with a specified name with authorization with default 1.0 version.
        /// </summary>
        /// <param name="builder">The endpoint route builder to extend.</param>
        /// <param name="name">The name of the route group.</param>
        /// <param name="configureApiVersionSetBuilder">An optional action to configure the API version set builder.</param>
        /// <returns>A <see cref="RouteGroupBuilder"/> for further configuration of the route group.</returns>

        public static RouteGroupBuilder MapBaseGroup(
            this IEndpointRouteBuilder builder,
            string name,
            Action<ApiVersionSetBuilder>? configureApiVersionSetBuilder = null)
        {
            return builder
                .MapGroup($"/api/v{{version:apiVersion}}/{name}")
                .RequireAuthorization()
                .WithApiVersionSet(builder.DefaultApiVersion(configureApiVersionSetBuilder).Build());
        }

        private static ApiVersionSetBuilder DefaultApiVersion(
            this IEndpointRouteBuilder builder,
            Action<ApiVersionSetBuilder>? configureApiVersionSetBuilder = null)
        {
            ApiVersionSetBuilder apiVersionSetBuilder = builder
                .NewApiVersionSet()
                .HasApiVersion(new ApiVersion(1.0))
                .ReportApiVersions();

            configureApiVersionSetBuilder?.Invoke(apiVersionSetBuilder);

            return apiVersionSetBuilder;
        }
    }
}
