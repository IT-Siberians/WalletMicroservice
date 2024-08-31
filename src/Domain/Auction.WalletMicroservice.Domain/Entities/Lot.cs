using Auction.Common.Domain.Entities;
using Auction.Common.Domain.EntitiesExceptions;
using Auction.Common.Domain.ValueObjects.String;
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
    public Lot(Guid id, Title title, Text description)
        : base(id, title, description)
    {
        GuidEmptyValueException.ThrowIfEmpty(id);
    }
}
