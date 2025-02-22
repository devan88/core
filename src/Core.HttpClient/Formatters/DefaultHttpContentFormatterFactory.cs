using System.Net.Http.Headers;
using Microsoft.Extensions.DependencyInjection;

namespace Core.HttpClient.Formatters
{
    internal sealed class DefaultHttpContentFormatterFactory : IHttpContentFormatterFactory
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultHttpContentFormatterFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc/>
        public IHttpContentFormatter CreateHttpContentFormatter(HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> mediaTypes)
        {
            ArgumentNullException.ThrowIfNull(mediaTypes);

            // Order the media types by quality of the media type (q-value), defaulting to 1
            // then select the first media type that we support.
            string? bestMediaType = mediaTypes
                .OrderByDescending(mt => mt.Quality ?? 1.0)
                .Select(mt => mt.MediaType)
                .FirstOrDefault();

            if (!string.IsNullOrWhiteSpace(bestMediaType) && MediaType.Xml.Contains(bestMediaType))
            {
                return _serviceProvider.GetRequiredService<XmlHttpContentFormatter>();
            }

            return _serviceProvider.GetRequiredService<JsonHttpContentFormatter>();
        }
    }
}
