namespace Auction.Common.Domain.Exceptions;

/// <summary>
/// Исключение домена для пустогой строки текста
/// </summary>
/// <param name="value">Значение текста</param>
public class TextEmptyValueException(string value)
    : DomainValidationException(
        $"The text cannot be an empty string or a space, the passed value is \"{value}\"");
