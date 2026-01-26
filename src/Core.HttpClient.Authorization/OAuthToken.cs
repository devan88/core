namespace Core.HttpClient.Authorization
{
    /// <summary>
    /// Represents a token from OAuth2 authentication.
    /// Contains various token properties received after a successful token request.
    /// </summary>
    public sealed record OAuthToken
    {
        /// <summary>
        /// Gets or sets the access token used to authorize subsequent API requests.
        /// </summary>
        public required string AccessToken { get; init; }

        /// <summary>
        /// Gets or sets the type of the token issued.
        /// Typically, this is "Bearer" to indicate a bearer token.
        /// </summary>
        public required string TokenType { get; init; }

        /// <summary>
        /// Gets or sets the lifetime in seconds of the access token.
        /// After this duration passes, the token will expire and a new one must be requested.
        /// </summary>
        public DateTimeOffset ExpiresOn { get; init; }
    }
}
