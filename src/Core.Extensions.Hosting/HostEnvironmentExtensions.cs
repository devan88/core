namespace Microsoft.Extensions.Hosting
{
    public static class HostEnvironmentExtensions
    {
        private static readonly string LocalDevelopment = "LocalDevelopment";

        public static bool IsLocalDevelopment(this IHostEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsEnvironment(LocalDevelopment);
        }
    }
}