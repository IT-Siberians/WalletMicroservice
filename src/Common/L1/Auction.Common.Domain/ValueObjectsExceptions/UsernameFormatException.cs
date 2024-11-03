using System;

namespace Auction.Common.Domain.ValueObjectsExceptions;

/// <summary>
/// Исключение домена для неправильного формата имени
/// </summary>
/// <param name="value">Значение имени</param>
internal class UsernameFormatException(string value, string format)
    : FormatException(
        $"The name is in the wrong format, the passed value is \"{value}\", it does not match the pattern {format}");
