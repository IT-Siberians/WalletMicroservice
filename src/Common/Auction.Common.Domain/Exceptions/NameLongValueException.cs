namespace Auction.Common.Domain.Exceptions;

public class NameLongValueException(string value, int maxLength)
    : DomainValidationException(
        $"The line must be no longer than {maxLength} characters, the passed value is \"{value}\"");
