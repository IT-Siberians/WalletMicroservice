namespace Auction.Common.Domain.Exceptions;


/// <summary>
/// Базовый класс доменных исключений валиадции аргументов 
/// </summary>
/// <param name="message">Сообщение исключения</param>
public class DomainValidationException(string message) : DomainException(message);
