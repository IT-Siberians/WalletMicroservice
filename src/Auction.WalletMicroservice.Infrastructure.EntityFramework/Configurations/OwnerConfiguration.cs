using Auction.Common.Domain.ValueObjects.String;
using Auction.WalletMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.WalletMicroservice.Infrastructure.EntityFramework.Configurations;

public class OwnerConfiguration : IEntityTypeConfiguration<Owner>
{
    public void Configure(EntityTypeBuilder<Owner> builder)
    {
        builder.Property(o => o.Username)
            .IsRequired()
            .HasMaxLength(PersonName.MaxLength)
            .HasConversion(
                name => name.Value,
                str => new PersonName(str)
            );

        builder.HasOne(o => o.Bill).WithOne(b => b.Owner);
    }
}
