namespace Auction.Common.Domain.Exceptions;

/// <summary>
/// Исключение домена для пустогой строки имени
/// </summary>
/// <param name="value">Значение имени</param>
public class NameEmptyValueException(string value)
    : DomainValidationException(
        $"The name cannot be an empty string or a space, the passed value is \"{value}\"");
