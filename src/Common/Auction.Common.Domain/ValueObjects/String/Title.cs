﻿using Auction.Common.Domain.ValueObjects.Abstract;
using Auction.Common.Domain.ValueObjectsExceptions;

namespace Auction.Common.Domain.ValueObjects.String;

/// <summary>
/// Объект значения заголовка.
/// Строка длиной от MinLength до MaxLength
/// </summary>
/// <param name="value">Значение заголовка</param>
public class Title(string value)
    : ValueObject<string>(value, Validate)
{
    public const int MinLength = 10;
    public const int MaxLength = 100;

    private static void Validate(string value)
    {
        if (value == null) throw new TitleNullValueException();
        if (string.IsNullOrWhiteSpace(value)) throw new TitleEmptyValueException(value);
        if (!IsCorrectLength(value)) throw new TitleLengthException(value, MinLength, MaxLength);

        if (!IsValid(value)) throw new ValidationInconsistencyException();
    }

    private static bool IsCorrectLength(string value) =>
        MinLength <= value.Length && value.Length <= MaxLength;

    public static bool IsValid(string value) =>
        !string.IsNullOrWhiteSpace(value)
        && IsCorrectLength(value);
}
