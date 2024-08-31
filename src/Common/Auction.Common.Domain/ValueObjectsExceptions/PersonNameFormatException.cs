using System;

namespace Auction.Common.Domain.ValueObjectsExceptions;

/// <summary>
/// Исключение домена для неправильного формата имени
/// </summary>
/// <param name="value">Значение имени</param>
public class PersonNameFormatException(string value)
    : FormatException(
        $"The name is in the wrong format, the passed value is \"{value}\"");
