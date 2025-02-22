using System.Net;
using Microsoft.Extensions.Options;

namespace Core.HttpClient.Authorization
{
    internal sealed class WindowsAuthHandler : HttpClientHandler
    {
        public readonly WindowsAuthConfiguration _windowsAuthConfiguration;

        public WindowsAuthHandler(IOptions<WindowsAuthConfiguration> options)
        {
            _windowsAuthConfiguration = options.Value;
            SetCrendentials();
        }

        private void SetCrendentials()
        {
            if (string.IsNullOrEmpty(_windowsAuthConfiguration.Username))
            {
                // This uses the credentials of the currently logged-on Windows user.
                UseDefaultCredentials = true;
            }
            else
            {
                Credentials = new NetworkCredential(
                    _windowsAuthConfiguration.Username,
                    _windowsAuthConfiguration.Password,
                    _windowsAuthConfiguration.Domain);
            }
        }
    }
}
