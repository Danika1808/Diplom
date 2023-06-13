namespace VerificationCenter.Model
{
    public class CreateSelfSignedCertificateCommand
    {
        public string OrganizationName { get; set; }
        public string OrganizationInn { get; set; }
        public DateTime ValidityPeriod { get; set; }
        public string Password { get; set; }
    }
}
