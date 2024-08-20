using Auction.Common.Domain.Entities;
using Auction.Common.Domain.ValueObjects;
using System;

namespace Auction.WalletMicroservice.Domain.Entities;

/// <summary>
/// Лот участвующий в транзакциях
/// </summary>
public class Lot : AbstractLot<Guid>
{
    /// <summary>
    /// Конструктор для EF
    /// </summary>
    protected Lot() : base() { }

    /// <summary>
    /// Основной конструктор лота
    /// </summary>
    /// <param name="id">Уникальный идентификатор лота</param>
    /// <param name="title">Название лота</param>
    /// <param name="description">Описание лота</param>
    public Lot(Guid id, Name title, Text description)
        : base(id, title, description)
    {
    }
}
