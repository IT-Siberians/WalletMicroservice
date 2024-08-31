using Auction.Common.Domain.ValueObjects.Abstract;
using Auction.Common.Domain.ValueObjectsExceptions;
using System;
using System.Numerics;

namespace Auction.Common.Domain.ValueObjects.Numeric;

/// <summary>
/// Объект значения количества денег.
/// Значение не может быть меньше 0
/// </summary>
/// <param name="value">Значение количества денег</param>
public class Money
    : NumericObject<decimal>,
    IAdditionOperators<Money, Money, Money>,
    ISubtractionOperators<Money, Money, Money>
{
    public Money(decimal value) : this(value, Validate) { }

    protected Money(decimal value, Action<decimal> validator) : base(value, validator) { }

    private static void Validate(decimal value)
    {
        if (value < 0) throw new MoneyNegativeValueException(value);

        if (!IsValid(value)) throw new ValidationInconsistencyException();
    }

    public static bool IsValid(decimal value) => value >= 0;

    public static Money operator +(Money left, Money right)
        => new(left.Value + right.Value);

    public static Money operator -(Money left, Money right)
        => new(left.Value - right.Value);
}
