using System;

namespace Auction.Common.Domain.ValueObjectsExceptions;

/// <summary>
/// Доменное исключение для неправильной длины строки имени
/// </summary>
/// <param name="value">Значение имени</param>
/// <param name="minLength">Минимальная допустимая длина строки</param>
/// <param name="maxLength">Максимальная допустимая длина строки</param>
internal class UsernameLengthException(string value, int minLength, int maxLength)
    : ArgumentException(
        $"The name must be no shoter than {minLength} and no longer than {maxLength} characters, current lenth {value.Length}, the passed value is \"{value}\"");
