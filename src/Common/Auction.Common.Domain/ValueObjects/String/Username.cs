using Auction.Common.Domain.ValueObjects.Abstract;
using Auction.Common.Domain.ValueObjectsExceptions;
using System.Text.RegularExpressions;

namespace Auction.Common.Domain.ValueObjects.String;

/// <summary>
/// Объект значения имени.
/// Строка длиной от MinLength до MaxLength
/// </summary>
/// <param name="value">Значение имени</param>
public class Username(string value)
    : StringValueObject(value, Validate)
{
    public const int MinLength = 3;
    public const int MaxLength = 30;

    public const string Pattern = "(^[a-zA-Z_-]+$)";
    public static readonly Regex ValidationRegex = new(Pattern, RegexOptions.Compiled);

    private static void Validate(string value)
    {
        if (value == null) throw new UsernameNullValueException();
        if (string.IsNullOrWhiteSpace(value)) throw new UsernameEmptyValueException(value);
        if (!IsCorrectLength(value)) throw new UsernameLengthException(value, MinLength, MaxLength);
        if (!ValidationRegex.IsMatch(value)) throw new UsernameFormatException(value, Pattern);

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
        && IsCorrectLength(value)
        && ValidationRegex.IsMatch(value);
}
