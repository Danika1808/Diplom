using WebVer.Domain.Identity;

namespace WebVer.Domain.Documents
{
    public class Document
    {
        public Guid Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public byte[] Data { get; set; }
    }
}
