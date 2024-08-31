using System;

namespace Auction.Common.Domain.ValueObjectsExceptions;

/// <summary>
/// Исключение домена для пустогой строки заголовка
/// </summary>
/// <param name="value">Значение заголовка</param>
public class TitleEmptyValueException(string value)
    : ArgumentException(
        $"The title cannot be an empty string or a space, the passed value is \"{value}\"");
