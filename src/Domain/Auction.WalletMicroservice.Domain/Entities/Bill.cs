using Auction.Common.Domain.Entities;
using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auction.WalletMicroservice.Domain.Entities;

/// <summary>
/// Счёт
/// </summary>
public class Bill : IEntity<Guid>
{
    private ICollection<Transfer>? _transfers;
    private ICollection<Freezing>? _freezings;

    /// <summary>
    /// Уникальный идентификатор счёта
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Владелец счёта
    /// </summary>
    public Owner Owner { get; }

    /// <summary>
    /// Количество денег на счету
    /// </summary>
    public Money Money { get; protected set; }

    /// <summary>
    /// Количество замороженных денег на счету
    /// </summary>
    public Money FrozenMoney { get; protected set; }

    /// <summary>
    /// Количество свободных денег на счету
    /// </summary>
    public Money FreeMoney => new Money(Money.Value - FrozenMoney.Value);

    /// <summary>
    /// Все переводы с участием счёта
    /// </summary>
    public IReadOnlyCollection<Transfer> Transfers => _transfers
        ?.ToList()
        ?? throw new FieldNullValueException(nameof(_transfers));

    /// <summary>
    /// Все замораживания/размораживания денег по счёту
    /// </summary>
    public IReadOnlyCollection<Freezing> Freezings => _freezings
        ?.ToList()
        ?? throw new FieldNullValueException(nameof(_freezings));

    /// <summary>
    /// Конструктор для EF
    /// </summary>
    protected Bill() { }

    /// <summary>
    /// Конструктор начального состояния счёта
    /// </summary>
    /// <param name="id">Уникальный идентификатор счёта</param>
    /// <param name="owner">Владелец счёта</param>
    public Bill(Guid id, Owner owner)
        : this(id, owner, new Money(0), new Money(0), [], [])
    {
    }

    /// <summary>
    /// Основной конструктор счёта
    /// </summary>
    /// <param name="id">Уникальный идентификатор счёта</param>
    /// <param name="owner">Владелец счёта</param>
    /// <param name="money">Количество денег на счету</param>
    /// <param name="frozenMoney">Количество замороженных денег на счету</param>
    /// <param name="transfers">Переводы с участием счёта</param>
    /// <param name="freezings">Заморозки/разморозки денег на счету</param>
    /// <exception cref="ArgumentNullValueException">Для null-аргументов</exception>
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

    /// <summary>
    /// Кладёт заданную сумму денег на счёт 
    /// </summary>
    /// <param name="money">Количество денег</param>
    public void PutMoney(Money money)
    {
        CheckMoney(money);

        Money = new Money(Money.Value + money.Value);
    }

    /// <summary>
    /// Снимает заданную сумму денег со счёта, если на счёте достаточно свободных денег
    /// </summary>
    /// <param name="money">Количество денег</param>
    /// <returns>true если на счёте достаточно свободных денег, иначе false</returns>
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

    /// <summary>
    /// Замораживает заданную сумму денег, если на счёте достаточно свободных денег
    /// </summary>
    /// <param name="money">Количество денег</param>
    /// <returns>true если на счёте достаточно свободных денег, иначе false</returns>
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

    /// <summary>
    /// Размораживает заданную сумму денег, если на счёте есть достаточная замороженная сумма
    /// </summary>
    /// <param name="money">Количество денег</param>
    /// <returns>true если на счёте достаточно замороженных денег, иначе false</returns>
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

    /// <summary>
    /// Добавяет транзакицю перевода с участием счёта
    /// </summary>
    /// <param name="transfer">Перевод между счетами</param>
    /// <exception cref="ArgumentNullValueException">Если аргумент null</exception>
    /// <exception cref="FieldNullValueException">Если поле null</exception>
    public void AddTransfer(Transfer transfer)
    {
        if (transfer == null) throw new ArgumentNullValueException(nameof(transfer));
        if (_transfers == null) throw new FieldNullValueException(nameof(_transfers));

        _transfers.Add(transfer);
    }

    /// <summary>
    /// Добавляет транзакцию заморозки/разморозки с участием счёта
    /// </summary>
    /// <param name="freezing">Заморозка/разморозка денег на счету</param>
    /// <exception cref="ArgumentNullValueException">Если аргумент null</exception>
    /// <exception cref="FieldNullValueException">Если поле null</exception>
    public void AddFreezing(Freezing freezing)
    {
        if (freezing == null) throw new ArgumentNullValueException(nameof(freezing));
        if (_freezings == null) throw new FieldNullValueException(nameof(_freezings));

        _freezings.Add(freezing);
    }
}
