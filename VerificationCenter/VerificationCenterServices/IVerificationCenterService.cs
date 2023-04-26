using System.Security.Cryptography.X509Certificates;
using VerificationCenter.Model;

namespace VerificationCenter.VerificationCenterServices
{
    public interface IVerificationCenterService
    {
        X509Certificate2 GenerateSelfSignedCertificate(CreateSelfSignedCertificateCommand request);

        X509Certificate2 GenerateIssueCertificate(string searchString,
            CreateIssuerCertificateCommand request);

        byte[] SignPdfDocument(string password, byte[] file, string userName);

        string GetSearchString(string name, string inn);
    }
}
