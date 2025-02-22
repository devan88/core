using System.Text;
using System.Xml.Serialization;

namespace Core.HttpClient.Formatters
{
    /// <summary>
    /// Xml formatter.
    /// </summary>
    public sealed class XmlHttpContentFormatter : IHttpContentFormatter
    {
        /// <inheritdoc/>
        public async Task<T?> DeserializeAsync<T>(HttpContent data, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(data);

            // Read the content string.
            string xmlContent = await data.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);

            if (string.IsNullOrWhiteSpace(xmlContent))
            {
                return default;
            }

            // Deserialize the XML to the target object.
            var xmlSerializer = new XmlSerializer(typeof(T));
            using (var reader = new StringReader(xmlContent))
            {
                return (T?)xmlSerializer.Deserialize(reader);
            }
        }

        /// <inheritdoc/>
        public HttpContent GetContent<T>(T data)
        {
            ArgumentNullException.ThrowIfNull(data);

            // Serialize the object to XML.
            XmlSerializer xmlSerializer = new(typeof(T));
            string xmlResult;
            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, data);
                xmlResult = stringWriter.ToString();
            }

            // Create an HttpContent instance with the proper media type.
            return new StringContent(xmlResult, Encoding.UTF8, "application/xml");
        }
    }
}
