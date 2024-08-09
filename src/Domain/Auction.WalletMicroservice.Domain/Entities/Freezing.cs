using Auction.Common.Domain.Entities;
using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;
using System;

namespace Auction.WalletMicroservice.Domain.Entities;

public class Freezing : IEntity<Guid>
{
    public Guid Id { get; protected set; }
    public bool IsUnfreezing { get; protected set; }

    private Bill? _bill;
    private Money? _money;
    private Lot? _lot;

    public Bill Bill => _bill ?? throw new FieldNullValueException(nameof(_bill));
    public Money Money => _money ?? throw new FieldNullValueException(nameof(_money));
    public Lot Lot => _lot ?? throw new FieldNullValueException(nameof(_lot));

    protected Freezing() { }

    public Freezing(Guid id, Bill bill, Money money, Lot lot, bool isUnfreezing)
    {
        _bill = bill ?? throw new ArgumentNullValueException(nameof(bill));
        _money = money ?? throw new ArgumentNullValueException(nameof(money));
        _lot = lot ?? throw new ArgumentNullValueException(nameof(lot));

        Id = id;
        IsUnfreezing = isUnfreezing;
    }
}
