using Auction.Common.Domain.ValueObjects.Numeric;
using Auction.WalletMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.WalletMicroservice.Infrastructure.EntityFramework.Configurations;

public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.Property(b => b.Money)
            .HasConversion(
                money => money.Value,
                number => new Money(number)
            );
        builder.Property(b => b.FrozenMoney)
            .HasConversion(
                money => money.Value,
                number => new Money(number)
            );

        builder.HasOne(b => b.Owner).WithOne(o => o.Bill);

        builder.HasMany<Transfer>("_transfers").WithOne(t => t.FromBill);
        builder.HasMany<Transfer>("_transfers").WithOne(t => t.ToBill);
        builder.HasMany<Freezing>("_freezings").WithOne(f => f.Bill);
    }
}
