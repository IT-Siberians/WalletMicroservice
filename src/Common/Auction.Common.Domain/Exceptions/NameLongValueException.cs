namespace Auction.Common.Domain.Exceptions;

/// <summary>
/// Доменное исключение для слишком длинной строки имени
/// </summary>
/// <param name="value">Значение имени</param>
/// <param name="maxLength">Максимальная допустимая длина строки</param>
public class NameLongValueException(string value, int maxLength)
    : DomainValidationException(
        $"The line must be no longer than {maxLength} characters, the passed value is \"{value}\"");
