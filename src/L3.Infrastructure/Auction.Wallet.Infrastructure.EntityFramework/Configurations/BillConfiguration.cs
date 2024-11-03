using Auction.WalletMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Wallet.Infrastructure.EntityFramework.Configurations;

internal class BillConfiguration : IEntityTypeConfiguration<Bill>
{
    public void Configure(EntityTypeBuilder<Bill> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Ignore(b => b.Money);
        builder.Ignore(b => b.TransfersTo);
        builder.Ignore(b => b.TransfersFrom);
        builder.Ignore(b => b.Freezings);

        builder.OwnsOne(
            b => b.FreeMoney,
            a => a.Property(m => m.Value)
                .HasColumnName("FreeMoney")
                .HasColumnType("money")
                .IsRequired());
        builder.OwnsOne(
            b => b.FrozenMoney,
            a => a.Property(m => m.Value)
                .HasColumnName("FrozenMoney")
                .HasColumnType("money")
                .IsRequired());

        builder.HasOne(b => b.Owner)
            .WithOne(o => o.Bill)
            .HasForeignKey<Bill>(b => b.OwnerId)
            .IsRequired();

        builder.HasMany<Transfer>("_transfersTo").WithOne(t => t.ToBill);
        builder.HasMany<Transfer>("_transfersFrom").WithOne(t => t.FromBill);
        builder.HasMany<Freezing>("_freezings").WithOne(f => f.Bill);
    }
}
