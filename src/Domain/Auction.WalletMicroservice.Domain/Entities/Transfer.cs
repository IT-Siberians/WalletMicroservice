using Auction.Common.Domain.Entities;
using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;

namespace Auction.WalletMicroservice.Domain.Entities;

public class Transfer : IEntity<Guid>
{
    public Guid Id { get; protected set; }
    public Bill? FromBill { get; protected set; }
    public Bill? ToBill { get; protected set; }
    public Lot? Lot { get; protected set; }

    private Money? _money;

    public Money Money => _money ?? throw new FieldNullValueException(nameof(_money));

    protected Transfer() { }

    public Transfer(Guid id, Money money, Bill fromBill, Bill toBill, Lot lot)
    {
        _money = money ?? throw new ArgumentNullValueException(nameof(money));
        FromBill = fromBill ?? throw new ArgumentNullValueException(nameof(fromBill));
        ToBill = toBill ?? throw new ArgumentNullValueException(nameof(toBill));
        Lot = lot ?? throw new ArgumentNullValueException(nameof(lot));

        Id = id;
    }

    public Transfer(Guid id, Money money, Bill? fromBill, Bill? toBill)
    {
        _money = money ?? throw new ArgumentNullValueException(nameof(money));

        Id = id;

        FromBill = fromBill;
        ToBill = toBill;
        Lot = null;
    }
}
