using Auction.Common.Domain.Entities;
using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;
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
    protected Owner() : base() { }

    /// <summary>
    /// Основной конструктор владельца счёта
    /// </summary>
    /// <param name="id">Уникальный идентификатор владельца</param>
    /// <param name="username">Имя владельца</param>
    /// <param name="bill">Счёт владельца</param>
    /// <exception cref="ArgumentNullValueException">Если аргумент null</exception>
    public Owner(Guid id, Name username, Bill bill)
        : base(id, username)
    {
        Bill = bill ?? throw new ArgumentNullValueException(nameof(bill));
    }
}
