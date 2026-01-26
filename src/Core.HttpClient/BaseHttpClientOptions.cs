using System.Collections.Concurrent;

namespace Core.HttpClient
{
    public record BaseHttpClientOptions
    {
        internal ConcurrentDictionary<Type, Func<IServiceProvider, IHttpContentFormatter>> HttpContentFormatterFactories { get; } = [];
    }
}
