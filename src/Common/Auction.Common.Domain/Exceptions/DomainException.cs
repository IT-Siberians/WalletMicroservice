using System;

namespace Auction.Common.Domain.Exceptions;

/// <summary>
/// Базовый класс доменных исключений
/// </summary>
/// <param name="message">Сообщение исключения</param>
public class DomainException(string message) : Exception(message);
