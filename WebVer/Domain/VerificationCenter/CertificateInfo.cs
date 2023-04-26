namespace WebVer.Domain.VerificationCenter
{
    public class CertificateInfo
    {
        public Guid Id { get; set; }

        public string SignatureAlgorithmName { get; set; }

        public DateTime NotAfter { get; set; }

        public DateTime NotBefore { get; set; }

        public string PublicKey { get; set; }

        public bool IsSelfSignedCertificate { get; set; }

        public string SubjectName { get; set; }
    }
}
