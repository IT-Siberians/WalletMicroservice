namespace Auction.Common.Domain.Exceptions;

/// <summary>
/// Исключение домена для null-значения поля
/// </summary>
/// <param name="fieldName">Имя поля</param>
public class FieldNullValueException(string fieldName)
    : DomainStateException(
        $"Field \"{fieldName}\" value is null");