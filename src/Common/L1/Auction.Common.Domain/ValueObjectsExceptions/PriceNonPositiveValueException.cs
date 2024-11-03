using System;

namespace Auction.Common.Domain.ValueObjectsExceptions;

/// <summary>
/// Исключение домена для неположительного значения цены
/// </summary>
/// <param name="value">Значение цены</param>
internal class PriceNonPositiveValueException(decimal value)
    : ArgumentException(
        $"The price value must be greater than 0, the passed value is: {value}");
