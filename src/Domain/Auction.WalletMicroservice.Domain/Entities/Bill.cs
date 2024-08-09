using Auction.Common.Domain.Entities;
using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;

namespace Auction.WalletMicroservice.Domain.Entities;

public class Bill : IEntity<Guid>
{
    public Guid Id { get; protected set; }

    private Owner? _owwner;
    private Money? _money;
    private Money? _frozenMoney;

    private ICollection<Transfer>? _transfers;
    private ICollection<Freezing>? _freezings;

    public Owner Owner => _owwner ?? throw new FieldNullValueException(nameof(_owwner));
    public Money Money => _money ?? throw new FieldNullValueException(nameof(_money));
    public Money FrozenMoney => _frozenMoney ?? throw new FieldNullValueException(nameof(_frozenMoney));
    public Money FreeMoney => new Money(Money.Value - FrozenMoney.Value);

    public IReadOnlyCollection<Transfer> Transfers => _transfers?.ToList() ?? throw new FieldNullValueException(nameof(_transfers));
    public IReadOnlyCollection<Freezing> Freezings => _freezings?.ToList() ?? throw new FieldNullValueException(nameof(_freezings));

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
        _owwner = owner ?? throw new ArgumentNullValueException(nameof(owner));
        _money = money ?? throw new ArgumentNullValueException(nameof(money));
        _frozenMoney = frozenMoney ?? throw new ArgumentNullValueException(nameof(frozenMoney));

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

        _money = new Money(Money.Value + money.Value);
    }

    public bool WithdrawMoney(Money money)
    {
        CheckMoney(money);

        if (FreeMoney.Value < money.Value)
        {
            return false;
        }

        _money = new Money(Money.Value - money.Value);

        return true;
    }

    public bool ReserveMoney(Money money)
    {
        CheckMoney(money);

        if (FreeMoney.Value < money.Value)
        {
            return false;
        }

        _money = new Money(Money.Value - money.Value);
        _frozenMoney = new Money(FrozenMoney.Value + money.Value);

        return true;
    }

    public bool RealeaseMoney(Money money)
    {
        CheckMoney(money);

        if (FrozenMoney.Value < money.Value)
        {
            return false;
        }

        _money = new Money(Money.Value + money.Value);
        _frozenMoney = new Money(FrozenMoney.Value - money.Value);

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
