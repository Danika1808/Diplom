using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System.Security.Cryptography.X509Certificates;
using iText.Kernel.Pdf;
using iText.Signatures;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Math;
using VerificationCenter.Model;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Crypto.Operators;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;
using VerificationCenter.CertificateServices;

namespace VerificationCenter.VerificationCenterServices
{
    public class VerificationCenterService : IVerificationCenterService
    {
        private readonly ICertificateService _certificateService;

        public VerificationCenterService(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        public X509Certificate2 GenerateSelfSignedCertificate(CreateSelfSignedCertificateCommand request)
        {
            var random = _certificateService.GetSecureRandom();

            var subjectKeyPair = _certificateService.GenerateKeyPair(random, 2048);

            var csr = _certificateService.CreateSelfSignedCertificateCsr(request, CryptographyAlgorithm.SHA256withRSA, subjectKeyPair);

            var csrInfo = csr.GetCertificationRequestInfo();

            var issuerKeyPair = subjectKeyPair;

            var serialNumber = _certificateService.GenerateSerialNumber(random);
            var issuerSerialNumber = serialNumber;

            const bool isCertificateAuthority = true;
            var certificate = _certificateService.GenerateCertificate(random, csrInfo.Subject, subjectKeyPair, serialNumber,
                csrInfo.Subject, issuerKeyPair,
                issuerSerialNumber, isCertificateAuthority, request.ValidityPeriod);

            var convertedCertificate = _certificateService.ConvertCertificate(certificate, subjectKeyPair, random, request.Password);

            _certificateService.SaveCertificateToStorage(convertedCertificate);

            return convertedCertificate;
        }

        public X509Certificate2 GenerateIssueCertificate(string searchString, CreateIssuerCertificateCommand request)
        {
            var issuerCertificate = _certificateService.GetCertificateFromStorageBySubjectName(searchString);
            var random = _certificateService.GetSecureRandom();

            var subjectKeyPair = _certificateService.GenerateKeyPair(random, 2048);

            var csr = _certificateService.CreateIssuerCertificateCsr(request, CryptographyAlgorithm.SHA256withRSA, subjectKeyPair);

            var csrInfo = csr.GetCertificationRequestInfo();

            var issuerKeyPair = DotNetUtilities.GetKeyPair(issuerCertificate.GetRSAPrivateKey());

            var serialNumber = _certificateService.GenerateSerialNumber(random);

            var issuerSerialNumber = new BigInteger(issuerCertificate.GetSerialNumber());

            var pfxBouncyCastleCertificate = DotNetUtilities.FromX509Certificate(issuerCertificate);

            const bool isCertificateAuthority = false;
            var certificate = _certificateService.GenerateCertificate(random,
                csrInfo.Subject, 
                subjectKeyPair,
                serialNumber,
                pfxBouncyCastleCertificate.SubjectDN, 
                issuerKeyPair,
                issuerSerialNumber, 
                isCertificateAuthority, DateTime.Now.AddYears(1));

            var convertedCertificate = _certificateService.ConvertCertificate(certificate, subjectKeyPair, random, request.Password);

            _certificateService.SaveCertificateToStorage(convertedCertificate);

            return convertedCertificate;
        }

        public byte[] SignPdfDocument(string password, byte[] file, string userName)
        {
            X509Certificate2 issuerCert = _certificateService.GetCertificateFromStorageBySubjectName(userName);

            byte[] rawdata = issuerCert.Export(X509ContentType.Pfx, password);
            var memStream = new MemoryStream(rawdata);
            var pk12 = new Pkcs12Store(memStream, password.ToCharArray());

            string alias = null;
            foreach (object a in pk12.Aliases)
            {
                alias = ((string)a);
                if (pk12.IsKeyEntry(alias))
                {
                    break;
                }
            }
            ICipherParameters pk = pk12.GetKey(alias).Key;

            X509CertificateEntry[] ce = pk12.GetCertificateChain(alias);
            X509Certificate[] chain = new X509Certificate[ce.Length];
            for (int k = 0; k < ce.Length; ++k)
            {
                chain[k] = ce[k].Certificate;

            }

            const string tempFilePath = "SignedPdf.pdf";

            using var stream = new MemoryStream(file);
            PdfReader reader = new(stream);
            using var fileStream = new FileStream(tempFilePath, FileMode.Create);
            PdfSigner signer = new(reader, fileStream, new());

            IExternalSignature pks = new PrivateKeySignature(pk, DigestAlgorithms.SHA256);

            signer.SignDetached(pks, chain, null, null, null, 0,
                PdfSigner.CryptoStandard.CMS);

            var result = File.ReadAllBytes(tempFilePath);

            File.Delete(tempFilePath);

            return result;
        }

        public string GetSearchString(string name, string inn)
        {
            return $"CN={name}, O={inn}";
        }
    }
}
