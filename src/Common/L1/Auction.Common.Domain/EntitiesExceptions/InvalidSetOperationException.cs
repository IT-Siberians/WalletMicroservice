using System;

namespace Auction.Common.Domain.EntitiesExceptions;

/// <summary>
/// Доменное исключение для невозможного использования сеттера
/// </summary>
/// <param name="argumentName">Имя аргумента</param>
/// <param name="argumentValue">Значение аргумента</param>
/// <param name="message">Сообщение</param>
public class InvalidSetOperationException<T>(string argumentName, T argumentValue, string message) : InvalidOperationException(message)
{
    /// <summary>
    /// Значение аргумента
    /// </summary>
    public T ArgumentValue { get; } = argumentValue;

    /// <summary>
    /// Имя аргумента
    /// </summary>
    public string ArgumentName { get; } = argumentName;
}
