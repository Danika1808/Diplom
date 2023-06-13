using WebVer.Domain.Identity;

namespace WebVer.Domain.Blockchain
{
    public class Event
    {
        public Guid Id { get; set; }
        public Guid DescriptionId { get; set; }
        public EventDescription Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid IssuerId { get; set; }
        public User Issuer { get; set; }
    }
}
