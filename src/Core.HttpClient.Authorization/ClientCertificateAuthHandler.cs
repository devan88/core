using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.Options;

namespace Core.HttpClient.Authorization
{
    public class ClientCertificateAuthHandler : HttpClientHandler
    {
        public readonly ClientCertificateAuthConfiguration _clientCertificateAuthConfiguration;

        public ClientCertificateAuthHandler(IOptions<ClientCertificateAuthConfiguration> options)
        {
            _clientCertificateAuthConfiguration = options.Value;

            X509Certificate2 clientCertificate = LoadCertificate();
            ClientCertificates.Add(clientCertificate);
        }

#if NET8_0
        private X509Certificate2 LoadCertificate()
        {
            X509Certificate2 clientCertificate = new(
                _clientCertificateAuthConfiguration.PathToCertificate,
                _clientCertificateAuthConfiguration.Password,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);

            return clientCertificate;
        }
#endif
#if NET9_0_OR_GREATER
        private X509Certificate2 LoadCertificate()
        {
            X509Certificate2 clientCertificate = X509CertificateLoader.LoadPkcs12FromFile(
                _clientCertificateAuthConfiguration.PathToCertificate,
                _clientCertificateAuthConfiguration.Password,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);

            return clientCertificate;
        }
#endif
    }
}
