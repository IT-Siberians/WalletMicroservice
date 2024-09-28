using Auction.Common.Domain.EntitiesExceptions;
using System;

namespace Auction.Common.Domain.ValueObjects.Abstract;

/// <summary>
/// Базовый объект значения
/// </summary>
/// <typeparam name="T">Тип значения</typeparam>
public abstract class ValueObject<T>
    : IValueObject<T>,
    IEquatable<ValueObject<T>>
{
    /// <summary>
    /// Значение
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Конструктор базового объекта значения
    /// </summary>
    /// <param name="value">Значение</param>
    /// <param name="validator">Функция валидации</param>
    /// <exception cref="ArgumentNullValueException">Если передан null</exception>
    protected ValueObject(T value, Action<T> validator)
    {
        if (value == null) throw new ArgumentNullValueException(nameof(value));
        if (validator == null) throw new ArgumentNullValueException(nameof(validator));

        validator(value);

        Value = value;
    }

    public override string? ToString() => Value!.ToString();

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }

        return Equals(obj as ValueObject<T>);
    }

    public bool Equals(ValueObject<T>? other)
    {
        if (ReferenceEquals(this, other))
        {
            return true;
        }
        if (other is null || other.GetType() != GetType())
        {
            return false;
        }

        return Equals(Value, other.Value);
    }

    public override int GetHashCode() => Value!.GetHashCode();
}
