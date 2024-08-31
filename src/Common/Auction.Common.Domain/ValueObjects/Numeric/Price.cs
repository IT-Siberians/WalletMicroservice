using Auction.Common.Domain.ValueObjectsExceptions;

namespace Auction.Common.Domain.ValueObjects.Numeric;

/// <summary>
/// Объект значения цены.
/// Значение не может быть меньше или равно 0.
/// От денег отличается запертом на значение 0
/// </summary>
/// <param name="value"></param>
public class Price(decimal value) : Money(value, Validate)
{
    private static void Validate(decimal value)
    {
        if (value <= 0) throw new PriceNonPositiveValueException(value);

        if (!IsValid(value)) throw new ValidationInconsistencyException();
    }

    public static new bool IsValid(decimal value) => value > 0;
}