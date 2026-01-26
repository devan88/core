using Microsoft.AspNetCore.Authorization;

namespace Core.Hosting.AspNetCore
{
    internal sealed class HeaderRequirement(string headerName, string expectedValue) : IAuthorizationRequirement
    {
        public string HeaderName { get; } = headerName;
        public string ExpectedValue { get; } = expectedValue;
    }
}
