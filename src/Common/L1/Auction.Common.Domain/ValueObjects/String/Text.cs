using Auction.Common.Domain.ValueObjects.Abstract;
using Auction.Common.Domain.ValueObjectsExceptions;

namespace Auction.Common.Domain.ValueObjects.String;

/// <summary>
/// Объект значения текста.
/// Строка любой длины
/// </summary>
/// <param name="value">Значение текста</param>
public class Text(string value)
    : StringValueObject(
        value,
        value =>
        {
            if (value is null) throw new TextNullValueException();
            if (string.IsNullOrWhiteSpace(value)) throw new TextEmptyValueException(value);

            if (!IsValid(value)) throw new ValidationInconsistencyException();
        })
{
    /// <summary>
    /// Проверяет, что значение можно перадать в конструктор
    /// </summary>
    /// <param name="value">Значение</param>
    /// <returns></returns>
    public static bool IsValid(string value) => !string.IsNullOrWhiteSpace(value);
}
