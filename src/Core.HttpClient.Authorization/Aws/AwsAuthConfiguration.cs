namespace Core.HttpClient.Authorization.Aws
{
    /// <summary>
    /// Represents the authorization configuration settings for AWS.
    /// Inherits from <see cref="OAuthConfiguration"/> and adds AWS-specific properties.
    /// </summary>
    public sealed record AwsAuthConfiguration : OAuthConfiguration
    {
        /// <summary>
        /// Gets or sets the AWS region.
        /// This is a required setting and specifies the region where AWS services are hosted.
        /// </summary>
        public required string Region { get; init; }

        /// <summary>
        /// Gets or sets the AWS domain.
        /// This is a required setting and specifies the domain used for AWS authentication.
        /// </summary>
        public required string Domain { get; init; }

        /// <summary>
        /// Gets the token endpoint URL for AWS Cognito OAuth2 authentication.
        /// The URL is dynamically constructed using the specified <see cref="Region"/> and <see cref="Domain"/>.
        /// </summary>
        public override string Endpoint => $"https://{Region}{Domain}.auth.{Region}.amazoncognito.com/oauth2/token";
    }
}
