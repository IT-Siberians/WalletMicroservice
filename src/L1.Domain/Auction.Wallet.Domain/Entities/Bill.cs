using Auction.Common.Domain.Entities;
using Auction.Common.Domain.EntitiesExceptions;
using Auction.Common.Domain.ValueObjects.Numeric;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auction.WalletMicroservice.Domain.Entities;

/// <summary>
/// Счёт
/// </summary>
public class Bill : IEntity<Guid>
{
#pragma warning disable IDE0044 // Add readonly modifier
    private ICollection<Transfer>? _transfersTo;
    private ICollection<Transfer>? _transfersFrom;
    private ICollection<Freezing>? _freezings;
#pragma warning restore IDE0044 // Add readonly modifier

    /// <summary>
    /// Уникальный идентификатор счёта
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Уникальный идентификатор владельца счёта
    /// </summary>
    public Guid OwnerId { get; }

    /// <summary>
    /// Владелец счёта
    /// </summary>
    public Owner Owner { get; }

    /// <summary>
    /// Количество свободных денег на счету
    /// </summary>
    public Money FreeMoney { get; protected set; }

    /// <summary>
    /// Количество замороженных денег на счету
    /// </summary>
    public Money FrozenMoney { get; protected set; }

    /// <summary>
    /// Количество всех денег на счету
    /// </summary>
    public Money Money => FreeMoney + FrozenMoney;

    /// <summary>
    /// Все переводы на счёт
    /// </summary>
    public IReadOnlyCollection<Transfer> TransfersTo => _transfersTo
        ?.ToList()
        ?? throw new FieldNullValueException(nameof(_transfersTo));

    /// <summary>
    /// Все переводы со счёта
    /// </summary>
    public IReadOnlyCollection<Transfer> TransfersFrom => _transfersFrom
        ?.ToList()
        ?? throw new FieldNullValueException(nameof(_transfersFrom));

    /// <summary>
    /// Все замораживания/размораживания денег по счёту
    /// </summary>
    public IReadOnlyCollection<Freezing> Freezings => _freezings
        ?.ToList()
        ?? throw new FieldNullValueException(nameof(_freezings));

    /// <summary>
    /// Конструктор для EF
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Bill() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>
    /// Конструктор начального состояния счёта
    /// </summary>
    /// <param name="id">Уникальный идентификатор счёта</param>
    /// <param name="owner">Владелец счёта</param>
    public Bill(Guid id, Owner owner)
        : this(id, owner, new Money(0), new Money(0), [], [], [])
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
        Money freeMoney,
        Money frozenMoney,
        ICollection<Transfer> transfersTo,
        ICollection<Transfer> transfersFrom,
        ICollection<Freezing> freezings)
    {
        Id = GuidEmptyValueException.GetGuidOrThrowIfEmpty(id);

        Owner = owner ?? throw new ArgumentNullValueException(nameof(owner));
        FreeMoney = freeMoney ?? throw new ArgumentNullValueException(nameof(freeMoney));
        FrozenMoney = frozenMoney ?? throw new ArgumentNullValueException(nameof(frozenMoney));

        OwnerId = owner.Id;

        _transfersTo = transfersTo ?? throw new ArgumentNullValueException(nameof(transfersTo));
        _transfersFrom = transfersFrom ?? throw new ArgumentNullValueException(nameof(transfersFrom));
        _freezings = freezings ?? throw new ArgumentNullValueException(nameof(freezings));
    }

    /// <summary>
    /// Кладёт заданную сумму денег на счёт 
    /// </summary>
    /// <param name="money">Количество денег</param>
    public void PutMoney(Money money)
    {
        ArgumentNullValueException.ThrowIfNull(money, nameof(money));

        FreeMoney += money;
    }

    /// <summary>
    /// Снимает заданную сумму денег со счёта, если на счёте достаточно свободных денег
    /// </summary>
    /// <param name="money">Количество денег</param>
    /// <returns>true если на счёте достаточно свободных денег, иначе false</returns>
    public bool WithdrawMoney(Money money)
    {
        ArgumentNullValueException.ThrowIfNull(money, nameof(money));

        if (FreeMoney < money)
        {
            return false;
        }

        FreeMoney -= money;

        return true;
    }

    /// <summary>
    /// Снимает заданную сумму денег со счёта, если на счёте достаточно зарезервированных денег
    /// </summary>
    /// <param name="price">Количество денег</param>
    /// <returns>true если на счёте достаточно зарезервированных денег, иначе false</returns>
    public bool PayForLot(Price price)
    {
        ArgumentNullValueException.ThrowIfNull(price, nameof(price));

        if (FrozenMoney < price)
        {
            return false;
        }

        FrozenMoney -= price;

        return true;
    }

    /// <summary>
    /// Замораживает заданную сумму денег, если на счёте достаточно свободных денег
    /// </summary>
    /// <param name="money">Количество денег</param>
    /// <returns>true если на счёте достаточно свободных денег, иначе false</returns>
    public bool ReserveMoney(Money money)
    {
        ArgumentNullValueException.ThrowIfNull(money, nameof(money));

        if (FreeMoney < money)
        {
            return false;
        }

        FreeMoney -= money;
        FrozenMoney += money;

        return true;
    }

    /// <summary>
    /// Размораживает заданную сумму денег, если на счёте есть достаточная замороженная сумма
    /// </summary>
    /// <param name="money">Количество денег</param>
    /// <returns>true если на счёте достаточно замороженных денег, иначе false</returns>
    public bool RealeaseMoney(Money money)
    {
        ArgumentNullValueException.ThrowIfNull(money, nameof(money));

        if (FrozenMoney < money)
        {
            return false;
        }

        FreeMoney += money;
        FrozenMoney -= money;

        return true;
    }

    /// <summary>
    /// Добавяет транзакицю перевода на счёт
    /// </summary>
    /// <param name="transfer">Перевод между счетами</param>
    /// <exception cref="ArgumentNullValueException">Если аргумент null</exception>
    /// <exception cref="FieldNullValueException">Если поле null</exception>
    public void AddTransferTo(Transfer transfer)
    {
        if (transfer is null) throw new ArgumentNullValueException(nameof(transfer));
        if (_transfersTo is null) throw new FieldNullValueException(nameof(_transfersTo));

        _transfersTo.Add(transfer);
    }

    /// <summary>
    /// Добавяет транзакицю перевода со счёта
    /// </summary>
    /// <param name="transfer">Перевод между счетами</param>
    /// <exception cref="ArgumentNullValueException">Если аргумент null</exception>
    /// <exception cref="FieldNullValueException">Если поле null</exception>
    public void AddTransferFrom(Transfer transfer)
    {
        if (transfer is null) throw new ArgumentNullValueException(nameof(transfer));
        if (_transfersFrom is null) throw new FieldNullValueException(nameof(_transfersFrom));

        _transfersFrom.Add(transfer);
    }

    /// <summary>
    /// Добавляет транзакцию заморозки/разморозки с участием счёта
    /// </summary>
    /// <param name="freezing">Заморозка/разморозка денег на счету</param>
    /// <exception cref="ArgumentNullValueException">Если аргумент null</exception>
    /// <exception cref="FieldNullValueException">Если поле null</exception>
    public void AddFreezing(Freezing freezing)
    {
        if (freezing is null) throw new ArgumentNullValueException(nameof(freezing));
        if (_freezings is null) throw new FieldNullValueException(nameof(_freezings));

        _freezings.Add(freezing);
    }
}
