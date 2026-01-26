using System.ComponentModel.DataAnnotations;

namespace Core.Hosting.AspNetCore
{
    /// <summary>
    /// Represents the options for JWT authentication configuration.
    /// </summary>
    public sealed record JwtAuthOptions
    {
        /// <summary>
        /// Gets or sets the authority URL for the JWT token issuer.
        /// This is a required field.
        /// </summary>
        [Required]
        public required string Authority { get; init; }

        /// <summary>
        /// Gets or sets the audience for which the JWT token is intended.
        /// This is a required field.
        /// </summary>
        [Required]
        public required string Audience { get; init; }

        /// <summary>
        /// Gets or sets the issuer of the JWT token.
        /// This field is optional.
        /// </summary>
        public string? Issuer { get; init; }

        /// <summary>
        /// Gets or sets a value indicating whether to validate the issuer of the JWT token.
        /// Default is true.
        /// </summary>
        public bool ValidateIssuer { get; init; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether to validate the lifetime of the JWT token.
        /// Default is true.
        /// </summary>
        public bool ValidateLifetime { get; init; } = true;

        /// <summary>
        /// Gets or sets a value indicating whether HTTPS metadata is required for the authority.
        /// Default is true.
        /// </summary>
        public bool RequiresHttpsMetadata { get; init; } = true;
    }
}
