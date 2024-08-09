namespace Auction.Common.Domain.Exceptions;

public class NameEmptyValueException(string value)
    : DomainValidationException(
        $"The name cannot be an empty string or a space, the passed value is \"{value}\"");
