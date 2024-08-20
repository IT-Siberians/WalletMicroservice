using Auction.Common.Domain.Exceptions;
using System;

namespace Auction.Common.Domain.ValueObjects;

/// <summary>
/// Базовый объект значения
/// </summary>
/// <typeparam name="T">Тип значения</typeparam>
public abstract class ValueObject<T>
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

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }
        if (obj == null || obj.GetType() != GetType())
        {
            return false;
        }

        return Equals(Value, ((ValueObject<T>)obj).Value);
    }

    public override int GetHashCode() => Value!.GetHashCode();

    public override string? ToString() => Value!.ToString();
}
