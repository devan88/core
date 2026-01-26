namespace Core.HttpClient
{
    public class HttpError
    {
        public int StatusCode { get; set; }

        public string? Message { get; set; }

        public string? TraceId { get; set; }

        public Dictionary<string, string[]>? ValidationErrors { get; set; }
    }
}
