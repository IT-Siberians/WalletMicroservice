using Auction.Common.Domain.Entities;
using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auction.WalletMicroservice.Domain.Entities;

public class Bill : IEntity<Guid>
{
    private ICollection<Transfer>? _transfers;
    private ICollection<Freezing>? _freezings;

    public Guid Id { get; }
    public Owner Owner { get; }

    public Money Money { get; protected set; }
    public Money FrozenMoney { get; protected set; }
    public Money FreeMoney => new Money(Money.Value - FrozenMoney.Value);

    public IReadOnlyCollection<Transfer> Transfers => _transfers
        ?.ToList()
        ?? throw new FieldNullValueException(nameof(_transfers));
    public IReadOnlyCollection<Freezing> Freezings => _freezings
        ?.ToList()
        ?? throw new FieldNullValueException(nameof(_freezings));

    protected Bill() { }

    public Bill(Guid id, Owner owner)
        : this(id, owner, new Money(0), new Money(0), [], [])
    {
    }

    public Bill(
        Guid id,
        Owner owner,
        Money money,
        Money frozenMoney,
        ICollection<Transfer> transfers,
        ICollection<Freezing> freezings)
    {
        Owner = owner ?? throw new ArgumentNullValueException(nameof(owner));
        Money = money ?? throw new ArgumentNullValueException(nameof(money));
        FrozenMoney = frozenMoney ?? throw new ArgumentNullValueException(nameof(frozenMoney));

        _transfers = transfers ?? throw new ArgumentNullValueException(nameof(transfers));
        _freezings = freezings ?? throw new ArgumentNullValueException(nameof(freezings));

        Id = id;
    }

    private static void CheckMoney(Money money)
    {
        if (money == null) throw new ArgumentNullValueException(nameof(money));
    }

    public void PutMoney(Money money)
    {
        CheckMoney(money);

        Money = new Money(Money.Value + money.Value);
    }

    public bool WithdrawMoney(Money money)
    {
        CheckMoney(money);

        if (FreeMoney.Value < money.Value)
        {
            return false;
        }

        Money = new Money(Money.Value - money.Value);

        return true;
    }

    public bool ReserveMoney(Money money)
    {
        CheckMoney(money);

        if (FreeMoney.Value < money.Value)
        {
            return false;
        }

        Money = new Money(Money.Value - money.Value);
        FrozenMoney = new Money(FrozenMoney.Value + money.Value);

        return true;
    }

    public bool RealeaseMoney(Money money)
    {
        CheckMoney(money);

        if (FrozenMoney.Value < money.Value)
        {
            return false;
        }

        Money = new Money(Money.Value + money.Value);
        FrozenMoney = new Money(FrozenMoney.Value - money.Value);

        return true;
    }

    public void AddTransfer(Transfer transfer)
    {
        if (transfer == null) throw new ArgumentNullValueException(nameof(transfer));
        if (_transfers == null) throw new FieldNullValueException(nameof(_transfers));

        _transfers.Add(transfer);
    }

    public void AddFreezing(Freezing freezing)
    {
        if (freezing == null) throw new ArgumentNullValueException(nameof(freezing));
        if (_freezings == null) throw new FieldNullValueException(nameof(_freezings));

        _freezings.Add(freezing);
    }
}
