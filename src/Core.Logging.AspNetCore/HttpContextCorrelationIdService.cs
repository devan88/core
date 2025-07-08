using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Core.Logging.AspNetCore
{
    /// <summary>
    /// Uses <see cref="IHttpContextAccessor"/> to get the correlation id.
    /// </summary>
    public sealed class HttpContextCorrelationIdService(IHttpContextAccessor httpContextAccessor) : ICorrelationIdService
    {
        private readonly IHttpContextAccessor _httpContextAccessor =
            httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));

        /// <inheritdoc/>
        public string GetCorrelationId()
        {
            HttpContext httpContext = _httpContextAccessor.HttpContext!;

            StringValues value = httpContext.Request.Headers[Constants.CorrelationIdHeader];

            return value == StringValues.Empty ? Guid.NewGuid().ToString() : value.ToString();
        }
    }
}
