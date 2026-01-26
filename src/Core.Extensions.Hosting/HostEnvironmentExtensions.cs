using Microsoft.Extensions.Hosting;

namespace Core.Extensions.Hosting
{
    /// <summary>
    /// Provides extension methods for <see cref="IHostEnvironment"/> interface to determine the hosting environment.
    /// </summary>
    public static class HostEnvironmentExtensions
    {
        /// <summary>
        /// The name of the local development environment.
        /// </summary>
        public static readonly string LocalDevelopment = "LocalDevelopment";

        /// <summary>
        /// Determines whether the current hosting environment is set to LocalDevelopment.
        /// </summary>
        /// <param name="hostingEnvironment">The hosting environment to check.</param>
        /// <returns><c>true</c> if the hosting environment is LocalDevelopment; otherwise, <c>false</c>.</returns>
        public static bool IsLocalDevelopment(this IHostEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsEnvironment(LocalDevelopment);
        }
    }
}
