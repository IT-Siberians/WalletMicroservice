using System;
using System.Numerics;

namespace Auction.Common.Domain.ValueObjects.Abstract;

/// <summary>
/// Базовый объект значения для чисел
/// </summary>
/// <typeparam name="T">Тип числа</typeparam>
public abstract class NumericValueObject<T>(T value, Action<T> validator)
    : ValueObject<T>(value, validator),
    IEqualityOperators<NumericValueObject<T>, NumericValueObject<T>, bool>,
    IComparisonOperators<NumericValueObject<T>, NumericValueObject<T>, bool>,
    IEquatable<NumericValueObject<T>>
        where T : struct, INumber<T>
{
    public bool Equals(NumericValueObject<T>? other) => base.Equals(other);

    public override bool Equals(object? other) => base.Equals(other);

    public override int GetHashCode() => base.GetHashCode();

    public static bool operator ==(NumericValueObject<T>? left, NumericValueObject<T>? right)
        => left?.Value == right?.Value;

    public static bool operator !=(NumericValueObject<T>? left, NumericValueObject<T>? right)
        => left?.Value != right?.Value;

    public static bool operator <(NumericValueObject<T> left, NumericValueObject<T> right)
        => left.Value < right.Value;

    public static bool operator >(NumericValueObject<T> left, NumericValueObject<T> right)
        => left.Value > right.Value;

    public static bool operator <=(NumericValueObject<T> left, NumericValueObject<T> right)
        => left.Value <= right.Value;

    public static bool operator >=(NumericValueObject<T> left, NumericValueObject<T> right)
        => left.Value >= right.Value;
}
