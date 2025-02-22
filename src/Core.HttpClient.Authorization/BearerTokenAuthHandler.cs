using System.Net.Http.Headers;

namespace Core.HttpClient.Authorization
{
    internal sealed class BearerTokenAuthHandler : DelegatingHandler
    {
        public readonly IOAuthTokenProviderService _oAuthTokenProviderService;

        public BearerTokenAuthHandler(IOAuthTokenProviderService oAuthTokenProviderService)
        {
            _oAuthTokenProviderService = oAuthTokenProviderService
                ?? throw new ArgumentNullException(nameof(oAuthTokenProviderService));
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
            string accessToken = await _oAuthTokenProviderService
                .GetAccessTokenAsync(cancellationToken)
                .ConfigureAwait(false);

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        }
    }

}
