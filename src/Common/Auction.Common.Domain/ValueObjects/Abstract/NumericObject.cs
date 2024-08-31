using System;
using System.Numerics;

namespace Auction.Common.Domain.ValueObjects.Abstract;

/// <summary>
/// Базовый объект значения для чисел
/// </summary>
/// <typeparam name="T">Тип числа</typeparam>
public abstract class NumericObject<T>(T value, Action<T> validator)
    : ValueObject<T>(value, validator),
    IEqualityOperators<NumericObject<T>, NumericObject<T>, bool>,
    IComparisonOperators<NumericObject<T>, NumericObject<T>, bool>,
    IEquatable<NumericObject<T>>
        where T : struct, INumber<T>
{
    public bool Equals(NumericObject<T>? other) => base.Equals(other);

    public override bool Equals(object? other) => base.Equals(other);

    public override int GetHashCode() => base.GetHashCode();

    public static bool operator ==(NumericObject<T>? left, NumericObject<T>? right)
        => left?.Value == right?.Value;

    public static bool operator !=(NumericObject<T>? left, NumericObject<T>? right)
        => left?.Value != right?.Value;

    public static bool operator <(NumericObject<T> left, NumericObject<T> right)
        => left.Value < right.Value;

    public static bool operator >(NumericObject<T> left, NumericObject<T> right)
        => left.Value > right.Value;

    public static bool operator <=(NumericObject<T> left, NumericObject<T> right)
        => left.Value <= right.Value;

    public static bool operator >=(NumericObject<T> left, NumericObject<T> right)
        => left.Value >= right.Value;
}
