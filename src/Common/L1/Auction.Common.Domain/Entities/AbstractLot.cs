using Auction.Common.Domain.EntitiesExceptions;
using Auction.Common.Domain.ValueObjects.String;
using System;

namespace Auction.Common.Domain.Entities;

/// <summary>
/// Базовый класс лота
/// </summary>
/// <typeparam name="TKey">Тип идентификатора</typeparam>
public abstract class AbstractLot<TKey> : IEntity<TKey>
    where TKey : struct, IEquatable<TKey>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public TKey Id { get; }

    /// <summary>
    /// Название лота
    /// </summary>
    public Title Title { get; protected set; }

    /// <summary>
    /// Описание лота
    /// </summary>
    public Text Description { get; protected set; }

    /// <summary>
    /// Конструктор для EF
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected AbstractLot() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>
    /// Основной конструктор лота
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="title">Название лота</param>
    /// <param name="description">Описание лота</param>
    /// <exception cref="ArgumentNullValueException">Для null-значений аргументов</exception>
    protected AbstractLot(TKey id, Title title, Text description)
    {
        Title = title ?? throw new ArgumentNullValueException(nameof(title));
        Description = description ?? throw new ArgumentNullValueException(nameof(description));

        Id = id;
    }
}
