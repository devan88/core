namespace Core.HttpClient.Authorization
{
    /// <summary>
    /// Represents a configuration for windows authorization.
    /// </summary>
    public sealed record WindowsAuthConfiguration
    {
        /// <summary>
        /// The username for the domain.
        /// </summary>
        public string? Username { get; init; }

        /// <summary>
        /// The password for the domain.
        /// </summary>
        public string? Password { get; init; }

        /// <summary>
        /// Domain of the network.
        /// </summary>
        public string? Domain { get; init; }
    }
}
