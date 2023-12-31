namespace Core.Hosting.AspNetCore.Swagger
{
    public class SwaggerDocOptions
    {
        public string Name { get; set; } = "v1";
        public OpenApiOptions OpenApiOptions { get; set; }

        public SwaggerDocOptions()
        {
            OpenApiOptions = new OpenApiOptions();
        }
    }
}
