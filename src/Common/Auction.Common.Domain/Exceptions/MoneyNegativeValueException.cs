using System;

namespace Auction.Common.Domain.Exceptions;

/// <summary>
/// Исключение домена для неправильного значения количества денег
/// </summary>
/// <param name="value">Значение количества денег</param>
public class MoneyNegativeValueException(decimal value)
    : ArgumentException(
        $"The money value cannot be less than 0, the passed value is: {value}");
