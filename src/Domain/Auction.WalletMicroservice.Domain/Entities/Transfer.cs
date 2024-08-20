using Auction.Common.Domain.Entities;
using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;
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
    protected Transfer() { }

    /// <summary>
    /// Конструктор транзакции перевода денег между счетами в оплату лота
    /// </summary>
    /// <param name="id">Уникальный идентификатор транзакции перевода</param>
    /// <param name="money">Количество переводимых денег</param>
    /// <param name="fromBill">Счёт с которого переводят деньги</param>
    /// <param name="toBill">Счёт на который переводят деньги</param>
    /// <param name="lot">Лот в оплату которого выполняется перевод</param>
    /// <exception cref="ArgumentNullValueException">Если аргумент null</exception>
    public Transfer(
        Guid id,
        Money money,
        Bill fromBill,
        Bill toBill,
        Lot lot)
    {
        Money = money ?? throw new ArgumentNullValueException(nameof(money));
        FromBill = fromBill ?? throw new ArgumentNullValueException(nameof(fromBill));
        ToBill = toBill ?? throw new ArgumentNullValueException(nameof(toBill));
        Lot = lot ?? throw new ArgumentNullValueException(nameof(lot));

        Id = id;
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
        Money = money ?? throw new ArgumentNullValueException(nameof(money));

        Id = id;

        FromBill = fromBill;
        ToBill = toBill;
        Lot = null;
    }
}
