namespace WebVer.Domain.Blockchain
{
    public class SmartContract
    {
        public Guid Id { get; set; }

        public List<Event>? Events { get; private set; } = null;
    }
}
