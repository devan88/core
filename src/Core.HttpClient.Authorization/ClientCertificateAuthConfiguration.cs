namespace Core.HttpClient.Authorization
{
    /// <summary>
    /// Represents a configuration for client credential authorization.
    /// </summary>
    public sealed record ClientCertificateAuthConfiguration
    {
        /// <summary>
        /// The path to the location of the certificate.
        /// </summary>
        public required string PathToCertificate { get; init; }

        /// <summary>
        /// The password for the certificate.
        /// </summary>
        public required string Password { get; init; }
    }
}
