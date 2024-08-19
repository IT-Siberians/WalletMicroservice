using Auction.Common.Domain.Entities;
using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;
using System;

namespace Auction.WalletMicroservice.Domain.Entities;

public class Transfer : IEntity<Guid>
{
    public Guid Id { get; }
    public Bill? FromBill { get; }
    public Bill? ToBill { get; }
    public Lot? Lot { get; }
    public Money Money { get; }

    protected Transfer() { }

    public Transfer(Guid id, Money money, Bill fromBill, Bill toBill, Lot lot)
    {
        Money = money ?? throw new ArgumentNullValueException(nameof(money));
        FromBill = fromBill ?? throw new ArgumentNullValueException(nameof(fromBill));
        ToBill = toBill ?? throw new ArgumentNullValueException(nameof(toBill));
        Lot = lot ?? throw new ArgumentNullValueException(nameof(lot));

        Id = id;
    }

    public Transfer(Guid id, Money money, Bill? fromBill, Bill? toBill)
    {
        Money = money ?? throw new ArgumentNullValueException(nameof(money));

        Id = id;

        FromBill = fromBill;
        ToBill = toBill;
        Lot = null;
    }
}
