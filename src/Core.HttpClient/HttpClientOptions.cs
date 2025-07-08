using System.ComponentModel.DataAnnotations;

namespace Core.HttpClient
{
    /// <summary>
    /// Represents the configuration settings for an Http client.
    /// </summary>
    public record HttpClientOptions
    {
        /// <summary>
        /// Gets or sets the Http client name which will be used when registering with IHttpClientFactory.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the base address for the Http client.
        /// </summary>
        [Required]
        public Uri BaseAddress { get; set; }

        /// <summary>
        /// Gets or sets the collection of headers to be sent with Http requests.
        /// </summary>
        public Dictionary<string, string> Headers { get; set; } = [];
    }
}
