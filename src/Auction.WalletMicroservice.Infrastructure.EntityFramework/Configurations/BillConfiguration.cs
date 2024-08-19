using Auction.Common.Domain.ValueObjects;
using Auction.WalletMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.WalletMicroservice.Infrastructure.EntityFramework.Configurations;

public class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.Property<Money>("_money")
            .HasConversion(
                money => money.Value,
                number => new Money(number)
            );
        builder.Property<Money>("_frozenMoney")
            .HasConversion(
                money => money.Value,
                number => new Money(number)
            );

        builder.HasOne<Owner>("_owwner").WithOne("_bill");
        builder.HasMany<Transfer>("_transfers").WithOne(t => t.FromBill);
        builder.HasMany<Transfer>("_transfers").WithOne(t => t.ToBill);
        builder.HasMany<Freezing>("_freezings").WithOne("_bill");
    }
}
