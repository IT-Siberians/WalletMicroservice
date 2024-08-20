using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.ValueObjects;
using System;

namespace Auction.Common.Domain.Entities;

/// <summary>
/// Базовый класс персоны
/// </summary>
/// <typeparam name="TKey">Тип идентификатора</typeparam>
public abstract class AbstractPerson<TKey> : IEntity<TKey>
    where TKey : notnull, IEquatable<TKey>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public TKey Id { get; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public Name Username { get; protected set; }

    /// <summary>
    /// Конструктор для EF
    /// </summary>
    protected AbstractPerson() { }

    /// <summary>
    /// Основной конструктор персоны
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="username">Имя пользователя</param>
    /// <exception cref="ArgumentNullValueException">Для null-значения аргумента</exception>
    protected AbstractPerson(TKey id, Name username)
    {
        Username = username ?? throw new ArgumentNullValueException(nameof(username));

        Id = id;
    }

    /// <summary>
    /// Изменяет имя пользователя
    /// </summary>
    /// <param name="username">Имя пользователя</param>
    /// <exception cref="ArgumentNullValueException">Для null-значения аргумента</exception>
    public virtual void ChangeUsername(Name username)
    {
        Username = username ?? throw new ArgumentNullValueException(nameof(username));
    }
}
