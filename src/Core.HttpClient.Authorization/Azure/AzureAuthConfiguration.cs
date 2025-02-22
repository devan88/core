namespace Core.HttpClient.Authorization.Azure
{
    /// <summary>
    /// Represents the authentication configuration settings for Azure.
    /// Inherits from <see cref="BearerTokenAuthConfiguration"/> and adds Azure-specific properties.
    /// </summary>
    public sealed record AzureAuthConfiguration : BearerTokenAuthConfiguration
    {
        /// <summary>
        /// Gets an empty instance of <see cref="AzureAuthConfiguration"/> with default values.
        /// This is useful for scenarios where you need to initialize a configuration object with defaults.
        /// Note that <see cref="CloudProvider"/> is set to Azure, and all the other values are initialized to empty strings.
        /// </summary>
        public static AzureAuthConfiguration Empty => new()
        {

            CloudProvider = CloudProvider.Azure,
            ClientId = string.Empty,
            ClientSecret = string.Empty,
            TenantId = string.Empty
        };

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

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureAuthConfiguration"/> class.
        /// The constructor ensures that the <see cref="CloudProvider"/> is set to Azure.
        /// </summary>
        public AzureAuthConfiguration()
        {
            CloudProvider = CloudProvider.Azure;
        }
    }
}
