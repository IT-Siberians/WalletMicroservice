using Auction.Common.Domain.Exceptions;

namespace Auction.Common.Domain.ValueObjects;

/// <summary>
/// Объект значения имени
/// </summary>
/// <param name="value">Значение имени</param>
public class Name(string value)
    : ValueObject<string>(
        value,
        value =>
        {
            if (value == null) throw new NameNullValueException();
            if (string.IsNullOrWhiteSpace(value)) throw new NameEmptyValueException(value);
            if (value.Length > MaxLength) throw new NameLongValueException(value, MaxLength);
        })
{
    public const int MaxLength = 50;
}
