namespace Auction.Common.Domain.Exceptions;

public class MoneyValueException(decimal value)
    : DomainValidationException(
        $"The money value cannot be less than 0, the passed value is: {value}");
