namespace VerificationCenter.Model
{
    public class GenerateCertificateRequest
    {
        public string Country { get; set; }
        public Guid UserId { get; set; }
        public string locality { get; set; }
        public string Organization { get; set; }
        public string OrganizationalUnit { get; set; }
        public string CommonName { get; set; }
        public string Password { get; set; }
    }
}
