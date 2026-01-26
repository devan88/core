using System.Collections.Immutable;

namespace Core.HttpClient
{
    /// <summary>
    /// Http client supported media types.
    /// </summary>
    internal static class MediaType
    {
        /// <summary>
        /// Readonly list for Xml media types.
        /// </summary>
        public static readonly ImmutableHashSet<string> Xml = ImmutableHashSet.Create(
             StringComparer.OrdinalIgnoreCase,
             "application/xml",
             "text/xml",
             "application/soap+xml");

        /// <summary>
        /// Readonly list for Json media types.
        /// </summary>
        public static readonly ImmutableHashSet<string> Json = ImmutableHashSet.Create(
            StringComparer.OrdinalIgnoreCase,
            "application/json",
            "text/json");

        /// <summary>
        /// Readonly list for MessagePack media types.
        /// </summary>
        public static readonly ImmutableHashSet<string> MessagePack = ImmutableHashSet.Create(
            StringComparer.OrdinalIgnoreCase,
            "application/x-msgpack",
            "application/msgpack",
            "application/vnd.msgpack");
    }
}
