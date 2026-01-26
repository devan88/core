namespace Core.Hosting.AspNetCore.Swagger
{ 
    /// <summary>
    /// Represents options for configuring OpenAPI documentation for an API.
    /// </summary>
    public record OpenApiOptions
    {
        /// <summary>
        /// Gets or sets the title of the API documentation.
        /// </summary>
        public string Title { get; set; } = "Default API";

        /// <summary>
        /// Gets or sets the description of the API documentation.
        /// </summary>
        public string Description { get; set; } = "Versioned API Using Swashbuckle and API Versioning";

    }
}
