using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebVer.Domain.Blockchain;

namespace WebVer.Configurations
{
    public class BlockConfiguration : IEntityTypeConfiguration<Block>
    {
        public void Configure(EntityTypeBuilder<Block> builder)
        {

            var block = new Block("", SmartContractConfiguration.SmartContractId);

            builder.HasData(block);
        }
    }
}
