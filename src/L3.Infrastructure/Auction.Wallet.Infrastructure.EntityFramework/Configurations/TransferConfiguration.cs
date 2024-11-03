using Auction.WalletMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Wallet.Infrastructure.EntityFramework.Configurations;

internal class TransferConfiguration : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.DateTime).IsRequired();

        builder.OwnsOne(
            t => t.Money,
            a => a.Property(m => m.Value)
                .HasColumnName("Money")
                .HasColumnType("money")
                .IsRequired());

        builder.HasOne(t => t.Lot).WithMany();
        builder.HasOne(t => t.FromBill).WithMany("_transfersFrom");
        builder.HasOne(t => t.ToBill).WithMany("_transfersTo");
    }
}
