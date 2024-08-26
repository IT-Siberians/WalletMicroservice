using System;

namespace Auction.Common.Domain.Exceptions;

/// <summary>
/// Доменное исключение для null-значения текста
/// </summary>
public class TextNullValueException()
    : ArgumentNullException("Text cannot be null");
