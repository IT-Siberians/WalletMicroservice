using Auction.Common.Domain.ValueObjects.String;
using Auction.WalletMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Wallet.Infrastructure.EntityFramework.Configurations;

internal class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.HasKey(o => o.Id);

        builder.Ignore(o => o.Balance);

        builder.OwnsOne(
            o => o.Username,
            a => a.Property(u => u.Value)
                .HasColumnName("Username")
                .HasMaxLength(Username.MaxLength)
                .IsRequired());

        builder.HasOne(o => o.Bill)
            .WithOne(b => b.Owner)
            .HasForeignKey<Owner>(o => o.BillId)
            .IsRequired();
    }
}
