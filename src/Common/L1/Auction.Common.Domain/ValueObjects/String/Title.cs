using Auction.Common.Domain.ValueObjects.Abstract;
using Auction.Common.Domain.ValueObjectsExceptions;

namespace Auction.Common.Domain.ValueObjects.String;

/// <summary>
/// Объект значения заголовка.
/// Строка длиной от MinLength до MaxLength
/// </summary>
/// <param name="value">Значение заголовка</param>
public class Title(string value)
    : StringValueObject(value, Validate)
{
    public const int MinLength = 3;
    public const int MaxLength = 50;

    private static void Validate(string value)
    {
        if (value is null) throw new TitleNullValueException();
        if (string.IsNullOrWhiteSpace(value)) throw new TitleEmptyValueException(value);
        if (!IsCorrectLength(value)) throw new TitleLengthException(value, MinLength, MaxLength);

        if (!IsValid(value)) throw new ValidationInconsistencyException();
    }

    private static bool IsCorrectLength(string value) =>
        MinLength <= value.Length && value.Length <= MaxLength;

    /// <summary>
    /// Проверяет, что значение можно перадать в конструктор
    /// </summary>
    /// <param name="value">Значение</param>
    /// <returns></returns>
    public static bool IsValid(string value) =>
        !string.IsNullOrWhiteSpace(value)
        && IsCorrectLength(value);
}
