using System;

namespace Auction.Common.Domain.Entities;

/// <summary>
/// Базовый интерфейс сущности
/// </summary>
/// <typeparam name="TKey">Тип уникального идентификатора</typeparam>
public interface IEntity<TKey>
    where TKey : notnull, IEquatable<TKey>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public TKey Id { get; }
}
