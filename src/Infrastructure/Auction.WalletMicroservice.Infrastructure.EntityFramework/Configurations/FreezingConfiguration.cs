using Auction.Common.Domain.ValueObjects.Numeric;
using Auction.WalletMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.WalletMicroservice.Infrastructure.EntityFramework.Configurations;

public class FreezingConfiguration : IEntityTypeConfiguration<Freezing>
{
    public void Configure(EntityTypeBuilder<Freezing> builder)
    {
        builder.Property(f => f.Money)
            .HasConversion(
                money => money.Value,
                number => new Money(number)
            );

        builder.HasOne(f => f.Lot).WithMany();
        builder.HasOne(f => f.Bill).WithMany("_freezings");
    }
}
