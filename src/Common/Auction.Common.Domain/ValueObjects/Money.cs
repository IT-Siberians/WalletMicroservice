using Auction.Common.Domain.Exceptions;

namespace Auction.Common.Domain.ValueObjects;

/// <summary>
/// Объект значения количества денег
/// </summary>
/// <param name="value">Значение количества денег</param>
public class Money(decimal value)
    : ValueObject<decimal>(
        value,
        value =>
        {
            if (value < 0) throw new MoneyValueException(value);
        });
