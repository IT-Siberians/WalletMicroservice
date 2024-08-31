using Auction.Common.Domain.ValueObjects.Abstract;
using Auction.Common.Domain.ValueObjectsExceptions;

namespace Auction.Common.Domain.ValueObjects.String;

/// <summary>
/// Объект значения текста.
/// Строка любой длины
/// </summary>
/// <param name="value">Значение текста</param>
public class Text(string value)
    : ValueObject<string>(
        value,
        value =>
        {
            if (value == null) throw new TextNullValueException();
            if (string.IsNullOrWhiteSpace(value)) throw new TextEmptyValueException(value);

            if (!IsValid(value)) throw new ValidationInconsistencyException();
        })
{
    public static bool IsValid(string value) => !string.IsNullOrWhiteSpace(value);
}
