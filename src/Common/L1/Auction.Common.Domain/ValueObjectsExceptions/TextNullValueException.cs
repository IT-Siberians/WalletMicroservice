using System;

namespace Auction.Common.Domain.ValueObjectsExceptions;

/// <summary>
/// Доменное исключение для null-значения текста
/// </summary>
internal class TextNullValueException()
    : ArgumentNullException("Text cannot be null");
