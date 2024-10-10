using Auction.Common.Domain.Entities;
using Auction.Common.Domain.EntitiesExceptions;
using Auction.Common.Domain.ValueObjects.Numeric;
using System;

namespace Auction.WalletMicroservice.Domain.Entities;

/// <summary>
/// Транзакция перевода между счетами
/// </summary>
public class Transfer : IEntity<Guid>
{
    /// <summary>
    /// Уникальный идентификатор транзакции перевода
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Двта и время транзакции
    /// </summary>
    public DateTime DateTime { get; }

    /// <summary>
    /// Счёт с которого переводят деньги
    /// </summary>
    public Bill? FromBill { get; }

    /// <summary>
    /// Счёт на который переводят деньги
    /// </summary>
    public Bill? ToBill { get; }

    /// <summary>
    /// Лот с которым связана транзакция перевода
    /// </summary>
    public Lot? Lot { get; }

    /// <summary>
    /// Количество переводимых денег
    /// </summary>
    public Money Money { get; }

    /// <summary>
    /// Конструктор для EF
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Transfer() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>
    /// Конструктор транзакции перевода денег между счетами в оплату лота
    /// </summary>
    /// <param name="id">Уникальный идентификатор транзакции перевода</param>
    /// <param name="price">Цена лота</param>
    /// <param name="fromBill">Счёт с которого переводят деньги</param>
    /// <param name="toBill">Счёт на который переводят деньги</param>
    /// <param name="lot">Лот в оплату которого выполняется перевод</param>
    /// <exception cref="ArgumentNullValueException">Если аргумент null</exception>
    public Transfer(
        Guid id,
        Price price,
        Bill fromBill,
        Bill toBill,
        Lot lot)
    {
        Id = GuidEmptyValueException.GetGuidOrThrowIfEmpty(id);

        Money = price ?? throw new ArgumentNullValueException(nameof(price));
        FromBill = fromBill ?? throw new ArgumentNullValueException(nameof(fromBill));
        ToBill = toBill ?? throw new ArgumentNullValueException(nameof(toBill));
        Lot = lot ?? throw new ArgumentNullValueException(nameof(lot));

        DateTime = DateTime.UtcNow;
    }

    /// <summary>
    /// Конструктор транзакции пополнения/снятия денег со счёта
    /// </summary>
    /// <param name="id">Уникальный идентификатор транзакции перевода</param>
    /// <param name="money">Количество переводимых денег</param>
    /// <param name="fromBill">Счёт с которого деньги снимаются</param>
    /// <param name="toBill">Счёт на который деньги кладутся</param>
    /// <exception cref="ArgumentNullValueException">Если количество денег равно null</exception>
    public Transfer(
        Guid id,
        Money money,
        Bill? fromBill,
        Bill? toBill)
    {
        Id = GuidEmptyValueException.GetGuidOrThrowIfEmpty(id);

        Money = money ?? throw new ArgumentNullValueException(nameof(money));

        FromBill = fromBill;
        ToBill = toBill;
        Lot = null;

        DateTime = DateTime.UtcNow;
    }
}
