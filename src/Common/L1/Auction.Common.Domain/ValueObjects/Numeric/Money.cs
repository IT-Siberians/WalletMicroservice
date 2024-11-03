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
    : NumericValueObject<decimal>,
    IAdditionOperators<Money, Money, Money>,
    ISubtractionOperators<Money, Money, Money>
{
    public Money(decimal value) : this(value, Validate) { }

    protected Money(decimal value, Action<decimal> validator)
        : base(Round(value), validator) { }

    /// <summary>
    /// Окргуляет до двух знаков после запятой
    /// </summary>
    /// <param name="value">Значение количества денег</param>
    /// <returns></returns>
    public static decimal Round(decimal value)
        => Math.Round(value, 2, MidpointRounding.ToPositiveInfinity);

    private static void Validate(decimal value)
    {
        if (value < 0) throw new MoneyNegativeValueException(value);

        if (!IsValid(value)) throw new ValidationInconsistencyException();
    }

    /// <summary>
    /// Проверяет, что значение можно перадать в конструктор
    /// </summary>
    /// <param name="value">Значение</param>
    /// <returns></returns>
    public static bool IsValid(decimal value) => value >= 0;

    public static Money operator +(Money left, Money right)
        => new(left.Value + right.Value);

    public static Money operator -(Money left, Money right)
        => new(left.Value - right.Value);
}
