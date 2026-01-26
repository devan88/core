using System.Linq;
using System.Net.Http.Headers;

namespace Core.HttpClient
{
    internal sealed class DefaultHttpContentFormatterStrategy : IHttpContentFormatterStrategy
    {
        public MediaTypeHeaderValue DefaultMediaTypeHeaderValue => new("application/json");

        public IHttpContentFormatter GetHttpContentFormatter(
            IEnumerable<IHttpContentFormatter> formatters,
            HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> mediaTypes)
        {
            ArgumentNullException.ThrowIfNull(mediaTypes);

            // Order the media types by quality of the media type (q-value), defaulting to 1
            // then select the first media type that is supported.
            string? bestMediaType = mediaTypes
                .OrderByDescending(mt => mt.Quality ?? 1.0)
                .Select(mt => mt.MediaType)
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(bestMediaType))
            {
                return formatters.First(formatter => formatter.MediaTypes.Contains(DefaultMediaTypeHeaderValue.MediaType!));
            }

            return formatters.First(formatter => formatter.MediaTypes.Contains(bestMediaType));
        }

        public IHttpContentFormatter GetHttpContentFormatter(
            IEnumerable<IHttpContentFormatter> formatters,
            MediaTypeHeaderValue? mediaType)
        {
            if (mediaType is null)
            {
                return formatters.First(formatter => formatter.MediaTypes.Contains(DefaultMediaTypeHeaderValue.MediaType!));
            }

            return formatters.First(formatter => formatter.MediaTypes.Contains(mediaType.MediaType!));
        }
    }
}
