namespace Auction.Common.Domain.Exceptions;

public class TextNullValueException()
    : DomainValidationException("Text cannot be null");
