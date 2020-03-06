using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EPiServer.Reference.Commerce.Site.Features.Payment.Services
{
    public class CertificateService
    {
        public static X509Certificate2 LoadMerchantCertificate(string certificateThumbprint)
        {
            X509Certificate2 certificate;

            // Load the certificate from the current user's certificate store. This
            // is useful if you do not want to publish the merchant certificate with
            // your application, but it is also required to be able to use an X.509
            // certificate with a private key if the user profile is not available,
            // such as when using IIS hosting in an environment such as Microsoft Azure.
            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);

                var certificates = store.Certificates.Find(
                    X509FindType.FindByThumbprint,
                    certificateThumbprint,
                    validOnly: false);

                if (certificates.Count < 1)
                {
                    throw new InvalidOperationException(
                        // ReSharper disable once UseStringInterpolation
                        string.Format(
                            "Could not find Apple Pay merchant certificate with thumbprint '{0}' from store '{1}' in location '{2}'.",
                            certificateThumbprint, store.Name, store.Location));
                }

                certificate = certificates[0];
            }

            return certificate;
        }

        public static string GetMerchantIdentifier(X509Certificate2 certificate)
        {
            // This OID returns the ASN.1 encoded merchant identifier
            var extension = certificate.Extensions["1.2.840.113635.100.6.32"];

            // Convert the raw ASN.1 data to a string containing the ID
            return extension == null ? string.Empty : Encoding.ASCII.GetString(extension.RawData).Substring(2);
        }
    }
}