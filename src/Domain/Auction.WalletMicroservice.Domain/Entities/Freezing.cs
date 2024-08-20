﻿using Auction.Common.Domain.Entities;
using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;
using System;

namespace Auction.WalletMicroservice.Domain.Entities;

/// <summary>
/// Транзакция заморозки/разморозки денег на счёте
/// </summary>
public class Freezing : IEntity<Guid>
{
    /// <summary>
    /// Уникальный идентификатор транзакции заморозки
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Тип транзакции: false - заморозка, true - разморозка
    /// </summary>
    public bool IsUnfreezing { get; }

    /// <summary>
    /// Счёт транзакции заморозки
    /// </summary>
    public Bill Bill { get; }

    /// <summary>
    /// Количество замораживаемых/размораживаемых денег
    /// </summary>
    public Money Money { get; }

    /// <summary>
    /// Лот для которого выполняется заморозка/разморозка
    /// </summary>
    public Lot Lot { get; }

    /// <summary>
    /// Конструктор для EF
    /// </summary>
    protected Freezing() { }

    /// <summary>
    /// Основной конструктор транзакции заморозки/разморозки денег на счёте
    /// </summary>
    /// <param name="id">Уникальный идентификатор транзакции</param>
    /// <param name="bill">Счёт транзакции</param>
    /// <param name="money">Количество денег для заморозки/разморозки</param>
    /// <param name="lot">Лот для которого выполняется заморозка/разморозка</param>
    /// <param name="isUnfreezing">Тип операции: false - заморозка, true - разморозка</param>
    /// <exception cref="ArgumentNullValueException">Если аргумент null</exception>
    public Freezing(Guid id, Bill bill, Money money, Lot lot, bool isUnfreezing)
    {
        Bill = bill ?? throw new ArgumentNullValueException(nameof(bill));
        Money = money ?? throw new ArgumentNullValueException(nameof(money));
        Lot = lot ?? throw new ArgumentNullValueException(nameof(lot));

        Id = id;
        IsUnfreezing = isUnfreezing;
    }
}
