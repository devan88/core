namespace Core.HttpClient.Authorization.Azure
{
    /// <summary>
    /// Represents the authentication configuration settings for Azure.
    /// Inherits from <see cref="OAuthConfiguration"/> and adds Azure-specific properties.
    /// </summary>
    public sealed record AzureAuthConfiguration : OAuthConfiguration
    {

        /// <summary>
        /// Gets the authority URL used for authentication against Azure AD.
        /// The Url is dynamically constructed using the provided <see cref="TenantId"/>.
        /// </summary>
        public string Authority => $"https://login.microsoftonline.com/{TenantId}";

        /// <summary>
        /// Gets or sets the tenant identifier for Azure Active Directory.
        /// This is a required setting that identifies the Azure AD tenant.
        /// </summary>
        public required string TenantId { get; init; }
    }
}
