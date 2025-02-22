using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Core.HttpClient.Formatters
{
    /// <summary>
    /// Json formatter.
    /// </summary>
    public sealed class JsonHttpContentFormatter : IHttpContentFormatter
    {
        /// <inheritdoc/>
        public Task<T?> DeserializeAsync<T>(HttpContent data, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(data);

            return data.ReadFromJsonAsync<T>(cancellationToken);
        }

        /// <inheritdoc/>
        public HttpContent GetContent<T>(T data)
        {
            ArgumentNullException.ThrowIfNull(data);

            string jsonData = JsonSerializer.Serialize(data);

            return new StringContent(jsonData, Encoding.UTF8, "application/json");
        }
    }
}
