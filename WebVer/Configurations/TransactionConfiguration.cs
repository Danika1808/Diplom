using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WebVer.Domain.Blockchain;

namespace WebVer.Configurations;

public class TransactionConfiguration : IEntityTypeConfiguration<Event>
{
    public static readonly Guid Id = Guid.Parse("b633ef18-5d27-480e-b7de-46455b648576");

    public void Configure(EntityTypeBuilder<Event> builder)
    {
        var transaction = new Event()
        {
            Id = Id
        };

        builder.HasData(transaction);
    }
}