namespace Core.HttpClient.Authorization
{
    /// <summary>
    /// Represents a configuration for API key authorization.
    /// </summary>
    public sealed record ApiKeyAuthConfiguration
    {
        /// <summary>
        /// Gets the name of the header that will contain the API key.
        /// </summary>
        public required string HeaderName { get; init; }

        /// <summary>
        /// Gets the API key used for authorization.
        /// </summary>
        public required string ApiKey { get; init; }
    }
}
