using System;

namespace Auction.Common.Domain.ValueObjectsExceptions;

/// <summary>
/// Доменное исключение для null-значения заголовка
/// </summary>
public class TitleNullValueException()
    : ArgumentNullException("Title cannot be null");
