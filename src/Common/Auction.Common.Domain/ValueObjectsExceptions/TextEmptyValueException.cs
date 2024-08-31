using System;

namespace Auction.Common.Domain.ValueObjectsExceptions;

/// <summary>
/// Исключение домена для пустогой строки текста
/// </summary>
/// <param name="value">Значение текста</param>
public class TextEmptyValueException(string value)
    : ArgumentException(
        $"The text cannot be an empty string or a space, the passed value is \"{value}\"");
