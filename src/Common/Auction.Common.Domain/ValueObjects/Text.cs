using Auction.Common.Domain.Exceptions;

namespace Auction.Common.Domain.ValueObjects;

/// <summary>
/// Объект значения текста
/// </summary>
/// <param name="value">Значение текста</param>
public class Text(string value)
    : ValueObject<string>(
        value,
        value =>
        {
            if (value == null) throw new TextNullValueException();
            if (string.IsNullOrWhiteSpace(value)) throw new TextEmptyValueException(value);
        });
