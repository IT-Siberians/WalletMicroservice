using Auction.Common.Domain.Entities;
using Auction.Common.Domain.EntitiesExceptions;
using Auction.Common.Domain.ValueObjects.Numeric;
using Auction.Common.Domain.ValueObjects.String;
using System;

namespace Auction.WalletMicroservice.Domain.Entities;

/// <summary>
/// Владелец счёта
/// </summary>
public class Owner : AbstractPerson<Guid>
{
    /// <summary>
    /// Счёт владельца
    /// </summary>
    public Bill Bill { get; }

    /// <summary>
    /// Количество денег на счёте
    /// </summary>
    public Money Balance => Bill.Money;

    /// <summary>
    /// Конструктор для EF
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected Owner() : base() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>
    /// Основной конструктор владельца счёта
    /// </summary>
    /// <param name="id">Уникальный идентификатор владельца</param>
    /// <param name="username">Имя владельца</param>
    /// <param name="bill">Счёт владельца</param>
    /// <exception cref="ArgumentNullValueException">Если аргумент null</exception>
    public Owner(Guid id, Username username, Bill bill)
        : base(id, username)
    {
        GuidEmptyValueException.ThrowIfEmpty(id);

        Bill = bill ?? throw new ArgumentNullValueException(nameof(bill));
    }
}
