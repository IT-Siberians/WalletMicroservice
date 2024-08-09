using Auction.Common.Domain.Exceptions;

namespace Auction.Common.Domain.ValueObjects;

public class Money(decimal value)
    : ValueObject<decimal>(
        value,
        value =>
        {
            if (value < 0) throw new MoneyValueException(value);
        });
