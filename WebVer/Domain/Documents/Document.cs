using WebVer.Domain.Identity;

namespace WebVer.Domain.Documents
{
    public class Document
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid IssuerId { get; set; }
        public User Issuer { get; set; }
        public Guid SignerId { get; set; }
        public User Signer { get; set; }
        public byte[] Data { get; set; }
        public bool IsSigned { get; set; }
    }
}
