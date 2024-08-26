using System;

namespace Auction.Common.Domain.Exceptions;

/// <summary>
/// Исключение домена для null-значения аргумента
/// </summary>
/// <param name="argumentName">Имя аргумента</param>
public class ArgumentNullValueException(string argumentName)
    : ArgumentNullException(
        argumentName,
        $"Argument \"{argumentName}\" value is null");
