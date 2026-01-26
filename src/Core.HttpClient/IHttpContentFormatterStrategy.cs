using System.Net.Http.Headers;

namespace Core.HttpClient
{
    /// <summary>
    /// Interface for getting an <see cref="IHttpContentFormatter"/> based on 
    /// a collection of media types defined in the Accept header of an Http request.
    /// </summary>
    public interface IHttpContentFormatterStrategy
    {
        /// <summary>
        /// Default media type.
        /// </summary>
        MediaTypeHeaderValue DefaultMediaTypeHeaderValue { get; }

        /// <summary>
        /// Gets an instance of an <see cref="IHttpContentFormatter"/> based on the provided media types.
        /// </summary>
        /// <param name="formatters">A list of <see cref="IHttpContentFormatter"/>.</param>
        /// <param name="mediaTypes">
        /// A collection of <see cref="MediaTypeWithQualityHeaderValue"/> representing the media types
        /// specified in the Http Accept header, each possibly including an associated quality factor (q-value)
        /// that indicates its preference.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IHttpContentFormatter"/> that corresponds to the best matching media type.
        /// </returns>
        IHttpContentFormatter GetHttpContentFormatter(
            IEnumerable<IHttpContentFormatter> formatters,
            HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> mediaTypes);

        /// <summary>
        /// Gets an instance of an <see cref="IHttpContentFormatter"/> based on the provided media type.
        /// </summary>
        /// <param name="formatters">A list of <see cref="IHttpContentFormatter"/>.</param>
        /// <param name="mediaType">A <see cref="MediaTypeHeaderValue"/> representing the media type.</param>
        /// An instance of <see cref="IHttpContentFormatter"/> that corresponds to the best matching media type.
        IHttpContentFormatter GetHttpContentFormatter(
            IEnumerable<IHttpContentFormatter> formatters,
            MediaTypeHeaderValue? mediaType);
    }
}
