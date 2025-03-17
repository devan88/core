namespace Core.Hosting.AspNetCore.Cors
{
    /// <summary>
    /// Cors configuration class.
    /// </summary>
    public sealed record CorsOptions
    {
        /// <summary>
        /// A list of allowed endpoints for the policy.
        /// </summary>
        public string[] AllowedEndpoints { get; set; } = [];
    }
}
