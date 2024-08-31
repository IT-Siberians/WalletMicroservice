using Auction.Common.Domain.ValueObjects.Abstract;
using Auction.Common.Domain.ValueObjectsExceptions;
using System.Text.RegularExpressions;

namespace Auction.Common.Domain.ValueObjects.String;

/// <summary>
/// Объект значения имени.
/// Строка длиной от MinLength до MaxLength
/// </summary>
/// <param name="value">Значение имени</param>
public class PersonName(string value)
    : ValueObject<string>(value, Validate)
{
    public const int MinLength = 3;
    public const int MaxLength = 30;

    public static readonly Regex ValidationRegex = new("(^[a-zA-Z_-]+$)", RegexOptions.Compiled);

    private static void Validate(string value)
    {
        if (value == null) throw new PersonNameNullValueException();
        if (string.IsNullOrWhiteSpace(value)) throw new PersonNameEmptyValueException(value);
        if (!IsCorrectLength(value)) throw new PersonNameLengthException(value, MinLength, MaxLength);
        if (!ValidationRegex.IsMatch(value)) throw new PersonNameFormatException(value);

        if (!IsValid(value)) throw new ValidationInconsistencyException();
    }

    private static bool IsCorrectLength(string value) =>
        MinLength <= value.Length && value.Length <= MaxLength;

    public static bool IsValid(string value) =>
        !string.IsNullOrWhiteSpace(value)
        && IsCorrectLength(value)
        && ValidationRegex.IsMatch(value);
}
