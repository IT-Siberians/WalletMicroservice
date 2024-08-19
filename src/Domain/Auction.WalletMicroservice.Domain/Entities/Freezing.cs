using Auction.Common.Domain.Entities;
using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;
using System;

namespace Auction.WalletMicroservice.Domain.Entities;

public class Freezing : IEntity<Guid>
{
    public Guid Id { get; }
    public bool IsUnfreezing { get; }
    public Bill Bill { get; }
    public Money Money { get; }
    public Lot Lot { get; }

    protected Freezing() { }

    public Freezing(Guid id, Bill bill, Money money, Lot lot, bool isUnfreezing)
    {
        Bill = bill ?? throw new ArgumentNullValueException(nameof(bill));
        Money = money ?? throw new ArgumentNullValueException(nameof(money));
        Lot = lot ?? throw new ArgumentNullValueException(nameof(lot));

        Id = id;
        IsUnfreezing = isUnfreezing;
    }
}
