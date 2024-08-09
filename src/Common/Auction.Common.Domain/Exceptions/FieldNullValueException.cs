namespace Auction.Common.Domain.Exceptions;

public class FieldNullValueException(string fieldName)
    : DomainStateException(
        $"Field \"{fieldName}\" value is null");