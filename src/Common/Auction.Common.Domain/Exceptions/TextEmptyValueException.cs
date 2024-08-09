namespace Auction.Common.Domain.Exceptions;

public class TextEmptyValueException(string value)
    : DomainValidationException(
        $"The text cannot be an empty string or a space, the passed value is \"{value}\"");
