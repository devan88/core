namespace Core.Hosting.AspNetCore.Swagger
{
    public class SwaggerOptions
    {
        public SwaggerDocOptions SwaggerDoc { get; set; }
        public SwaggerEndpointOptions SwaggerEndpoint { get; set; }

        public SwaggerOptions()
        {
            SwaggerDoc = new SwaggerDocOptions();
            SwaggerEndpoint = new SwaggerEndpointOptions();
        }
    }
}
