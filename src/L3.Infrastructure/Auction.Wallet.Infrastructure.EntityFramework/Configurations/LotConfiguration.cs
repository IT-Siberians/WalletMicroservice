using Auction.Common.Domain.ValueObjects.String;
using Auction.WalletMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.Wallet.Infrastructure.EntityFramework.Configurations;

internal class LotConfiguration : IEntityTypeConfiguration<Lot>
{
    public void Configure(EntityTypeBuilder<Lot> builder)
    {
        builder.HasKey(e => e.Id);

        builder.OwnsOne(
            e => e.Title,
            a => a.Property(t => t.Value)
                .HasColumnName("Title")
                .HasMaxLength(Title.MaxLength)
                .IsRequired());
        builder.OwnsOne(
            e => e.Description,
            a => a.Property(t => t.Value)
                .HasColumnName("Description")
                .IsRequired());
    }
}