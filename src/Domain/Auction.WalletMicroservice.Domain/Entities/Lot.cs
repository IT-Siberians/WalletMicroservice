using Auction.Common.Domain.Entities;
using Auction.Common.Domain.ValueObjects;
using System;

namespace Auction.WalletMicroservice.Domain.Entities;

public class Lot : AbstractLot<Guid>
{
    protected Lot() : base(Guid.Empty) { }

    public Lot(Guid id, Name title, Text description)
        : base(id, title, description)
    {
    }
}
