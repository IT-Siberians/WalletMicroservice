using System;
using System.Numerics;

namespace Auction.Common.Domain.ValueObjects.Abstract;

public abstract class StringValueObject(string value, Action<string> validator)
    : ValueObject<string>(value, validator),
    IEqualityOperators<StringValueObject, StringValueObject, bool>,
    IComparisonOperators<StringValueObject, StringValueObject, bool>,
    IEquatable<StringValueObject>
{
    public override string? ToString() => Value!;

    public bool Equals(StringValueObject? other)
        => StringComparer.Ordinal.Equals(this, other);

    public override bool Equals(object? other)
        => StringComparer.Ordinal.Equals(this, other);

    public override int GetHashCode()
        => StringComparer.Ordinal.GetHashCode(this);

    public static bool operator ==(StringValueObject? left, StringValueObject? right)
        => StringComparer.Ordinal.Equals(left, right);

    public static bool operator !=(StringValueObject? left, StringValueObject? right)
        => !(left == right);

    public static bool operator <(StringValueObject left, StringValueObject right)
        => StringComparer.Ordinal.Compare(left, right) < 0;

    public static bool operator >(StringValueObject left, StringValueObject right)
        => StringComparer.Ordinal.Compare(left, right) > 0;

    public static bool operator <=(StringValueObject left, StringValueObject right)
        => StringComparer.Ordinal.Compare(left, right) <= 0;

    public static bool operator >=(StringValueObject left, StringValueObject right)
        => StringComparer.Ordinal.Compare(left, right) >= 0;
}
