namespace Auction.Common.Domain.Exceptions;

public class ArgumentNullValueException(string argumentName)
    : DomainValidationException(
        $"Argument \"{argumentName}\" value is null");
