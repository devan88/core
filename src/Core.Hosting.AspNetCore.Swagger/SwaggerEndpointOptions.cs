namespace Core.Hosting.AspNetCore.Swagger
{
    public class SwaggerEndpointOptions
    {
        public string Url { get; set; } = "v1/swagger.json";
        public string Name { get; set; } = "SwaggerApi v1";
    }
}
