using System.Text.Json;
using Core.HttpClient.Formatters;

namespace Core.HttpClient.Extensions
{
    /// <summary>
    /// Provides extension methods for BaseHttpClientBuilder.
    /// </summary>
    public static class BaseHttpClientBuilderExtensions
    {
        /// <summary>
        /// Adds <see cref="JsonHttpContentFormatter"/> into the available http formatters.
        /// </summary>
        /// <param name="builder">The <see cref="IBaseHttpClientBuilder"/> to add the json http formatter</param>
        /// <param name="options">Optional <see cref="JsonSerializerOptions"/> configuration.</param>
        /// <returns>Updated <see cref="IBaseHttpClientBuilder"/>.</returns>
        public static IBaseHttpClientBuilder AddJsonHttpContentFormatter(
            this IBaseHttpClientBuilder builder,
            Action<JsonSerializerOptions>? options = null)
        {
            return builder
                .AddHttpContentFormatter<JsonHttpContentFormatter, JsonSerializerOptions>(options);
        }
    }
}
