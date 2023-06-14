namespace WebVer.Domain.Blockchain
{
    public class SmartContract
    {
        public Guid Id { get; set; }

        public List<Event>? Events { get; internal set; } = null;

        public bool IsCompleted { get; internal set; }
    }
}
