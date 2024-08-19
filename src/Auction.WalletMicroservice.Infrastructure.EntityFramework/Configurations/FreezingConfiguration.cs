using Auction.Common.Domain.ValueObjects;
using Auction.WalletMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.WalletMicroservice.Infrastructure.EntityFramework.Configurations;

public class FreezingConfiguration : IEntityTypeConfiguration<Freezing>
{
    public void Configure(EntityTypeBuilder<Freezing> builder)
    {
        builder.Property<Money>("_money")
            .HasConversion(
                money => money.Value,
                number => new Money(number)
            );

        builder.HasOne<Bill>("_bill").WithMany("_freezings");
        builder.HasOne<Lot>("_lot").WithMany();
    }
}
