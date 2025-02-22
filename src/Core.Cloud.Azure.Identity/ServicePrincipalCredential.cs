namespace Core.Cloud.Azure.Identity
{
    /// <summary>
    /// A service principal record for cloud authentication
    /// </summary>
    public record ServicePrincipalCredential
    {
        /// <summary>
        /// Name of the principal.
        /// </summary>
        public string Name { get; init; }

        /// <summary>
        /// Tenant id.
        /// </summary>
        public string TenantId { get; init; }

        /// <summary>
        /// Client id of the principal
        /// </summary>
        public string ClientId { get; init; }

        /// <summary>
        /// The secret key name in the cloud.
        /// </summary>
        public virtual string ClientSecretKey => $"{Name}-Secret";
    }
}
