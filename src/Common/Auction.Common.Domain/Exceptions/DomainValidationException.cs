namespace Auction.Common.Domain.Exceptions;

public class DomainValidationException(string message) : DomainException(message);
