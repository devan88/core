using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.Options;

namespace Core.HttpClient.Authorization
{
    internal sealed class BasicAuthHandler : DelegatingHandler
    {
        public readonly BasicAuthConfiguration _basicAuthConfiguration;

        public BasicAuthHandler(IOptions<BasicAuthConfiguration> options)
        {
            _basicAuthConfiguration = options.Value;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Create the basic auth credential string.
            string credential = Convert
                .ToBase64String(Encoding.ASCII.GetBytes($"{_basicAuthConfiguration.Username}:{_basicAuthConfiguration.Password}"));

            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credential);

            return base.SendAsync(request, cancellationToken);
        }
    }
}
