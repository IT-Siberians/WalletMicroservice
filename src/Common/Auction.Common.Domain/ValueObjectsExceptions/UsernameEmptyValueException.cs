using System;

namespace Auction.Common.Domain.ValueObjectsExceptions;

/// <summary>
/// Исключение домена для пустогой строки имени
/// </summary>
/// <param name="value">Значение имени</param>
internal class UsernameEmptyValueException(string value)
    : ArgumentException(
        $"The name cannot be an empty string or a space, the passed value is \"{value}\"");
