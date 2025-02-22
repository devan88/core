using System.Text.Json.Serialization;

namespace Core.HttpClient.Authorization.Aws
{
    /// <summary>
    /// Represents a token response from AWS Cognito OAuth2 authentication.
    /// Contains various token properties received after a successful token request.
    /// </summary>
    public sealed record AwsCognitoTokenResponse
    {
        /// <summary>
        /// Gets or sets the access token used to authorize subsequent API requests.
        /// </summary>
        [JsonPropertyName("access_token")]
        public string? AccessToken { get; init; }

        /// <summary>
        /// Gets or sets the type of the token issued.
        /// Typically, this is "Bearer" to indicate a bearer token.
        /// </summary>
        [JsonPropertyName("token_type")]
        public string? TokenType { get; init; }

        /// <summary>
        /// Gets or sets the lifetime in seconds of the access token.
        /// After this duration passes, the token will expire and a new one must be requested.
        /// </summary>
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; init; }

        /// <summary>
        /// Gets or sets the ID token, which often contains user identity information.
        /// This token is optional and may not be returned unless explicitly requested.
        /// </summary>
        [JsonPropertyName("id_token")]
        public string? IdToken { get; init; }
    }
}
