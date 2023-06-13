using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebVer.Domain.Blockchain;

namespace WebVer.Configurations;

public class SmartContractConfiguration : IEntityTypeConfiguration<SmartContract>
{
    public static Guid SmartContractId = Guid.Parse("94976d58-5245-4834-9863-30ab8c363679");

    public void Configure(EntityTypeBuilder<SmartContract> builder)
    {
        var contract = new SmartContract()
        {
            Id = SmartContractId
        };

        builder.HasData(contract);
    }
}