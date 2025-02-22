namespace Core.HttpClient
{
    /// <summary>
    /// Represents the configuration settings for an Http client.
    /// </summary>
    public record HttpClientConfiguration
    {
        /// <summary>
        /// Gets or sets the base address for the Http client.
        /// This property is required and must be initialized.
        /// </summary>
        public required Uri BaseAddress { get; init; }

        /// <summary>
        /// Gets or sets the collection of headers to be sent with Http requests.
        /// This property is initialized to an empty dictionary by default.
        /// </summary>
        public Dictionary<string, string> Headers { get; init; } = [];
    }
}
