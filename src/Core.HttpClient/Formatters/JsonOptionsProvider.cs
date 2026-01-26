using System.Text.Json;
using System.Text.Json.Serialization;

namespace Core.HttpClient.Formatters
{
    internal static class JsonOptionsProvider
    {
        internal static JsonSerializerOptions Default => new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            WriteIndented = false,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters =
            {
                new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
            },
        };
    }
}
