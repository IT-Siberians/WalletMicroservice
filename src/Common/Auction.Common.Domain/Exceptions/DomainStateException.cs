namespace Auction.Common.Domain.Exceptions;

/// <summary>
/// Базовый класс доменных исключений неверного состояния объектов
/// </summary>
/// <param name="message">Сообщение исключения</param>
public class DomainStateException(string message) : DomainException(message);
