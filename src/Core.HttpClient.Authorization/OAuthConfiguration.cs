using System.ComponentModel.DataAnnotations;

namespace Core.HttpClient.Authorization
{
    /// <summary>
    /// The http client authorization configuration to support OAuth client credentials flow.
    /// </summary>
    public record OAuthConfiguration
    {

        /// <summary>
        /// Gets or sets the client ID used for authentication.
        /// This is a required setting used to uniquely identify the client
        /// application during the authentication process.
        /// </summary>
        [Required]
        public required string ClientId { get; init; }

        /// <summary>
        /// Gets or sets the client secret used for authentication.
        /// This is a required setting and must be kept secure as it verifies the identity
        /// of the client application along with the client ID.
        /// </summary>
        [Required]
        public required string ClientSecret { get; init; }

        /// <summary>
        /// Gets or sets the scopes for which the authentication token is valid.
        /// Scopes determine the level of access and permissions the token will have.
        /// Defaults to an empty array if no scopes are provided.
        /// </summary>
        public string[] Scopes { get; init; } = [];

        /// <summary>
        /// Gets the endpoint URL OAuth2 authentication.
        /// </summary>
        public virtual string Endpoint { get; init; } = string.Empty;
    }
}
