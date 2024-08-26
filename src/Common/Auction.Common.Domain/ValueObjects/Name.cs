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
            if (value.Length < MinLength
                || value.Length > MaxLength) throw new NameLengthException(value, MinLength, MaxLength);
        })
{
    public const int MinLength = 3;
    public const int MaxLength = 30;
}
