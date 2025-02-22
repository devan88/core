namespace Core.HttpClient.Authorization
{
    /// <summary>
    /// Represents a configuration for basic Http authorization.
    /// </summary>
    public sealed record BasicAuthConfiguration
    {
        /// <summary>
        /// The username for basic authorization.
        /// </summary>
        public required string Username { get; init; }

        /// <summary>
        /// The password for basic authorization.
        /// </summary>
        public required string Password { get; init; }
    }
}
