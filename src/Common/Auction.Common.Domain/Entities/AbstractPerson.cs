using Auction.Common.Domain.EntitiesExceptions;
using Auction.Common.Domain.ValueObjects.String;
using System;

namespace Auction.Common.Domain.Entities;

/// <summary>
/// Базовый класс персоны
/// </summary>
/// <typeparam name="TKey">Тип идентификатора</typeparam>
public abstract class AbstractPerson<TKey> : IEntity<TKey>, IDeletableSoftly
    where TKey : struct, IEquatable<TKey>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public TKey Id { get; }

    /// <summary>
    /// Имя пользователя
    /// </summary>
    public PersonName Username { get; protected set; }

    /// <summary>
    /// Является ли пользователь удалённым
    /// </summary>
    public bool IsDeletedSoftly { get; protected set; }

    /// <summary>
    /// Конструктор для EF
    /// </summary>
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    protected AbstractPerson() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    /// <summary>
    /// Основной конструктор персоны
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <param name="username">Имя пользователя</param>
    /// <exception cref="ArgumentNullValueException">Для null-значения аргумента</exception>
    protected AbstractPerson(TKey id, PersonName username)
    {
        Username = username ?? throw new ArgumentNullValueException(nameof(username));

        Id = id;
    }

    /// <summary>
    /// Помечает пользователя как удалённого
    /// </summary>
    public void MarkAsDeletedSoftly() => IsDeletedSoftly = true;

    /// <summary>
    /// Изменяет имя пользователя
    /// </summary>
    /// <param name="username">Имя пользователя</param>
    /// <exception cref="ArgumentNullValueException">Для null-значения аргумента</exception>
    public virtual void ChangeUsername(PersonName username)
    {
        Username = username ?? throw new ArgumentNullValueException(nameof(username));
    }
}
