using System.Security.Cryptography;
using System.Text;

namespace WebVer.Domain.Blockchain
{
    public class Block
    {
        public Guid Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string PreviousHash { get; set; }
        public Guid SmartContractId { get; set; }
        public SmartContract SmartContract { get; set; }
        public string Hash { get; set; }
        public int Nonce { get; set; }

        private Block(){}

        public Block(string previousHash, Guid smartContractId)
        {
            Id = Guid.NewGuid();
            TimeStamp = DateTime.Now;
            PreviousHash = previousHash;
            Hash = CalculateHash();
            SmartContractId = smartContractId;
            MineBlock(4);
        }

        private string CalculateHash()
        {
            using var sha256 = SHA256.Create();
            var rawData = Id + TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.fff") + PreviousHash + SmartContract?.Id + Nonce;
            var bytes = Encoding.UTF8.GetBytes(rawData);
            var hashBytes = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

        public void MineBlock(int difficulty)
        {
            var target = new string('0', difficulty);
            while (Hash[..difficulty] != target)
            {
                Nonce++;
                Hash = CalculateHash();
            }
        }
    }
}
