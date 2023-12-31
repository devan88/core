using Azure.Core;
using Azure.Identity;

namespace Core.Extensions.Azure
{
    public record CredentialOptions
    {
        private static Func<IServiceProvider, TokenCredential> LocalDevelopmentCredential => (provider) => new ChainedTokenCredential(
            new DefaultAzureCredential());
        private static Func<IServiceProvider, TokenCredential> DevelopmentCredential => (provider) => new ChainedTokenCredential(
            new EnvironmentCredential(),
            new WorkloadIdentityCredential(),
            new ManagedIdentityCredential());
        private static Func<IServiceProvider, TokenCredential> StagingCredential => (provider) => new ChainedTokenCredential(
            new EnvironmentCredential(),
            new WorkloadIdentityCredential(),
            new ManagedIdentityCredential());
        private static Func<IServiceProvider, TokenCredential> ProductionCredential => (provider) => new ChainedTokenCredential(
            new EnvironmentCredential(),
            new WorkloadIdentityCredential(),
            new ManagedIdentityCredential());

        public Func<IServiceProvider, TokenCredential> LocalDevelopment { get; set; } = LocalDevelopmentCredential;
        public Func<IServiceProvider, TokenCredential> Development { get; set; } = DevelopmentCredential;
        public Func<IServiceProvider, TokenCredential> Staging { get; set; } = StagingCredential;
        public Func<IServiceProvider, TokenCredential> Production { get; set; } = ProductionCredential;

        public CredentialOptions() { }
    }
}
