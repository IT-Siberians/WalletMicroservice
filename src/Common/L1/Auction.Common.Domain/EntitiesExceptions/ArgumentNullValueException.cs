using System;

namespace Auction.Common.Domain.EntitiesExceptions;

/// <summary>
/// Исключение домена для null-значения аргумента
/// </summary>
/// <param name="argumentName">Имя аргумента</param>
public class ArgumentNullValueException(string argumentName)
    : ArgumentNullException(
        argumentName,
        $"Argument \"{argumentName}\" value is null")
{
    /// <summary>
    /// Бросает исключение, если значение аргумента равно null
    /// </summary>
    /// <param name="value">Значение аргумента</param>
    /// <param name="argumentName">Имя аргумента</param>
    /// <exception cref="GuidEmptyValueException">Если аргумент равен пустому Guid</exception>
    public static new void ThrowIfNull(object? value, string argumentName)
    {
        if (value is null) throw new ArgumentNullValueException(argumentName);
    }
}
