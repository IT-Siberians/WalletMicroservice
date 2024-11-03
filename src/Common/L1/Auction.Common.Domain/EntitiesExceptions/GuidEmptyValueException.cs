using System;

namespace Auction.Common.Domain.EntitiesExceptions;

/// <summary>
/// Исключение домена для пустого Guid
/// </summary>
public class GuidEmptyValueException() : ArgumentException("Guid is empty")
{
    /// <summary>
    /// Бросает исключение, если значение аргумента равно пустому Guid
    /// </summary>
    /// <param name="guid">Значение Guid</param>
    /// <exception cref="GuidEmptyValueException">Если аргумент равен пустому Guid</exception>
    public static void ThrowIfEmpty(Guid guid)
    {
        if (guid == Guid.Empty) throw new GuidEmptyValueException();
    }

    /// <summary>
    /// Бросает исключение, если значение аргумента равно пустому Guid.
    /// В противном случае возвращает переданное значение Guid
    /// </summary>
    /// <param name="guid">Значение Guid</param>
    /// <exception cref="GuidEmptyValueException">Если аргумент равен пустому Guid</exception>
    public static Guid GetGuidOrThrowIfEmpty(Guid guid)
    {
        ThrowIfEmpty(guid);

        return guid;
    }
}
