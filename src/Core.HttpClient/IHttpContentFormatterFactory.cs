using System.Net.Http.Headers;

namespace Core.HttpClient
{
    /// <summary>
    /// Factory interface for creating an <see cref="IHttpContentFormatter"/> based on 
    /// a collection of media types defined in the Accept header of an Http request.
    /// </summary>
    public interface IHttpContentFormatterFactory
    {
        /// <summary>
        /// Creates an instance of an <see cref="IHttpContentFormatter"/> based on the provided media types.
        /// </summary>
        /// <param name="mediaTypes">
        /// A collection of <see cref="MediaTypeWithQualityHeaderValue"/> representing the media types
        /// specified in the Http Accept header, each possibly including an associated quality factor (q-value)
        /// that indicates its preference.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IHttpContentFormatter"/> that corresponds to the best matching media type.
        /// Typically, this returns an Xml formatter if an Xml media type is preferred;
        /// otherwise, it falls back to a Json formatter.
        /// </returns>
        IHttpContentFormatter CreateHttpContentFormatter(HttpHeaderValueCollection<MediaTypeWithQualityHeaderValue> mediaTypes);
    }
}
