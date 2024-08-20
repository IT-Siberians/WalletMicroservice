namespace Auction.Common.Domain.Exceptions;

/// <summary>
/// Доменное исключение для null-значения текста
/// </summary>
public class TextNullValueException()
    : DomainValidationException("Text cannot be null");
