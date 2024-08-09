namespace Auction.Common.Domain.Exceptions;

public class NameNullValueException()
    : DomainValidationException("Name cannot be null");
