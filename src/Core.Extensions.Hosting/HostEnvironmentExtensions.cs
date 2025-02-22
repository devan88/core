namespace Microsoft.Extensions.Hosting
{
    public static class HostEnvironmentExtensions
    {
        private static readonly string s_localDevelopment = "LocalDevelopment";

        public static bool IsLocalDevelopment(this IHostEnvironment hostingEnvironment)
        {
            return hostingEnvironment.IsEnvironment(s_localDevelopment);
        }
    }
}
