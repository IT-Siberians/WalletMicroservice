﻿using Auction.Common.Domain.ValueObjects.Numeric;
using Auction.WalletMicroservice.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auction.WalletMicroservice.Infrastructure.EntityFramework.Configurations;

public class TransferConfiguration : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> builder)
    {
        builder.Property(t => t.Money)
            .HasConversion(
                money => money.Value,
                number => new Money(number)
            );

        builder.HasOne(t => t.Lot).WithMany();
        builder.HasOne(t => t.FromBill).WithMany("_transfers");
        builder.HasOne(t => t.ToBill).WithMany("_transfers");
    }
}
