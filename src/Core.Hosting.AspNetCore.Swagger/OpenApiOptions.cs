namespace Core.Hosting.AspNetCore.Swagger
{
    public record OpenApiOptions
    {
        public string Title { get; set; } = "SwaggerApi";
        public string Version { get; set; } = "v1";

    }
}
