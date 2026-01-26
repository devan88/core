using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Core.Hosting.AspNetCore
{
    internal sealed class HeaderRequirementHandler : AuthorizationHandler<HeaderRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            HeaderRequirement requirement)
        {
            HttpContext? httpContext = context.Resource switch
            {
                HttpContext ctx => ctx,
                AuthorizationFilterContext filterContext => filterContext.HttpContext,
                _ => null
            };

            if (httpContext is null)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var headers = httpContext.Request.Headers;
            if(headers.TryGetValue(requirement.HeaderName.ToLower(), out var actualValue)
                && actualValue.ToString() == requirement.ExpectedValue)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
