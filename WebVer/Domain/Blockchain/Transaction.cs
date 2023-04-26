using WebVer.Domain.Identity;

namespace WebVer.Domain.Blockchain
{
    public class Transaction
    {
        public Guid Id { get; set; }
        public TransactionDescription Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid IssuerId { get; set; }
        public User Issuer { get; set;}
    }
}
