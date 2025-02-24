namespace Core.HttpClient
{
    /// <summary>
    /// Represents the configuration settings for an Http client.
    /// </summary>
    public record HttpClientConfiguration
    {
        /// <summary>
        /// Gets or sets the Http client name which will be used when registering with IHttpClientFactory.
        /// </summary>
        public required string Name { get; init; }

        /// <summary>
        /// Gets or sets the base address for the Http client.
        /// </summary>
        public required Uri BaseAddress { get; init; }

        /// <summary>
        /// Gets or sets the collection of headers to be sent with Http requests.
        /// </summary>
        public Dictionary<string, string> Headers { get; init; } = [];
    }
}
