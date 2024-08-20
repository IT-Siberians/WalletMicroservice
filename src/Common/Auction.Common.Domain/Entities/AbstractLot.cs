using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;
using System;

namespace Auction.Common.Domain.Entities;

/// <summary>
/// Базовый класс лота
/// </summary>
/// <typeparam name="TKey">Тип идентификатора</typeparam>
public abstract class AbstractLot<TKey> : IEntity<TKey>
    where TKey : notnull, IEquatable<TKey>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public TKey Id { get; }

    /// <summary>
    /// Название лота
    /// </summary>
    public Name Title { get; protected set; }

    /// <summary>
    /// Описание лота
    /// </summary>
    public Text Description { get; protected set; }

    /// <summary>
    /// Конструктор для EF
    /// </summary>
    protected AbstractLot() { }

    /// <summary>
    /// Основной конструктор лота
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="title">Название лота</param>
    /// <param name="description">Описание лота</param>
    /// <exception cref="ArgumentNullValueException">Для null-значений аргументов</exception>
    protected AbstractLot(TKey id, Name title, Text description)
    {
        Title = title ?? throw new ArgumentNullValueException(nameof(title));
        Description = description ?? throw new ArgumentNullValueException(nameof(description));

        Id = id;
    }
}
