using System.Net.Http.Headers;

namespace Core.HttpClient.Authorization
{
    internal sealed class BearerTokenAuthHandler : DelegatingHandler
    {
        public readonly IOAuthTokenProvider _oAuthTokenProvider;

        public BearerTokenAuthHandler(IOAuthTokenProvider oAuthTokenProvider)
        {
            _oAuthTokenProvider = oAuthTokenProvider
                ?? throw new ArgumentNullException(nameof(oAuthTokenProvider));
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            await SetAuthorizationHeader(request, cancellationToken);

            return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
        }

        private async Task SetAuthorizationHeader(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            string accessToken = (await _oAuthTokenProvider
                .GetAccessTokenAsync(cancellationToken)
                .ConfigureAwait(false)).AccessToken!;

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }

}
