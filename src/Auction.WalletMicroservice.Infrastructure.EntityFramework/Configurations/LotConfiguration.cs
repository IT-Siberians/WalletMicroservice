using Auction.Common.Domain.ValueObjects;
using Auction.WalletMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.WalletMicroservice.Infrastructure.EntityFramework.Configurations;

public class LotConfiguration : IEntityTypeConfiguration<Lot>
{
    public void Configure(EntityTypeBuilder<Lot> builder)
    {
        builder.Property(e => e.Title)
            .IsRequired()
            .HasMaxLength(Name.MaxLength)
            .HasConversion(
                name => name.Value,
                str => new Name(str)
            );
        builder.Property(e => e.Description)
            .IsRequired()
            .HasConversion(
                text => text.Value,
                str => new Text(str)
            );
    }
}