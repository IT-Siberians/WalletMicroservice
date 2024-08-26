using System;

namespace Auction.Common.Domain.Exceptions;

/// <summary>
/// Исключение домена для null-значения поля
/// </summary>
/// <param name="fieldName">Имя поля</param>
public class FieldNullValueException(string fieldName)
    : ArgumentNullException(
        fieldName,
        $"Field \"{fieldName}\" value is null");