using Auction.WalletMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Wallet.Infrastructure.EntityFramework.Configurations;

internal class FreezingConfiguration : IEntityTypeConfiguration<Freezing>
{
    public void Configure(EntityTypeBuilder<Freezing> builder)
    {
        builder.HasKey(f => f.Id);

        builder.Property(f => f.DateTime).IsRequired();
        builder.Property(f => f.IsUnfreezing).IsRequired();

        builder.OwnsOne(
            f => f.Money,
            a => a.Property(m => m.Value)
                .HasColumnName("Money")
                .HasColumnType("money")
                .IsRequired());

        builder.HasOne(f => f.Lot).WithMany();
        builder.HasOne(f => f.Bill).WithMany("_freezings");
    }
}
