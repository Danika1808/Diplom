using Org.BouncyCastle.Crypto.Digests;
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
        private Block(){}

        public Block(string previousHash, Guid smartContractId)
        {
            Id = Guid.NewGuid();
            TimeStamp = DateTime.Now;
            PreviousHash = previousHash;
            Hash = CalculateHash();
            SmartContractId = smartContractId;
        }

        private string CalculateHash()
        {
            var rawData = Id + TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.fff") + PreviousHash + SmartContract?.Id;
            
            var bytes = Encoding.UTF8.GetBytes(rawData);

            Gost3411Digest gost3411 = new Gost3411Digest();
            gost3411.BlockUpdate(bytes, 0, bytes.Length);
            byte[] hashBytes = new byte[gost3411.GetDigestSize()];
            gost3411.DoFinal(hashBytes, 0);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }
    }
}
