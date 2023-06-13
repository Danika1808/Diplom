﻿using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Math;
using System.Security.Cryptography.X509Certificates;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;
using Org.BouncyCastle.Pkcs;
using VerificationCenter.Model;

namespace VerificationCenter.CertificateServices
{
    public interface ICertificateService
    {
        X509Certificate GenerateCertificate(SecureRandom random,
            X509Name subjectDn,
            AsymmetricCipherKeyPair subjectKeyPair,
            BigInteger subjectSerialNumber,
            X509Name issuerDn,
            AsymmetricCipherKeyPair issuerKeyPair,
            BigInteger issuerSerialNumber,
            bool isCertificateAuthority,
            DateTime endDate);

        X509Certificate2 ConvertCertificate(X509Certificate certificate,
            AsymmetricCipherKeyPair subjectKeyPair,
            SecureRandom random,
            string password);

        AsymmetricCipherKeyPair GenerateKeyPair(SecureRandom random, int strength);

        SecureRandom GetSecureRandom();

        BigInteger GenerateSerialNumber(SecureRandom random);

        Pkcs10CertificationRequest CreateSelfSignedCertificateCsr(CreateSelfSignedCertificateCommand request,
            CryptographyAlgorithm algorithm,
            AsymmetricCipherKeyPair keyPair);

        Pkcs10CertificationRequest CreateIssuerCertificateCsr(CreateIssuerCertificateCommand request,
            CryptographyAlgorithm algorithm,
            AsymmetricCipherKeyPair keyPair);

        void SaveCertificateToStorage(X509Certificate2 certificate);

        X509Certificate2 GetCertificateFromStorageBySubjectName(string searchString);

        bool CertificateIsExist(string subjectName);
    }
}
