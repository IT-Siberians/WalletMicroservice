using System;

namespace Auction.Common.Domain.Exceptions;

/// <summary>
/// Доменное исключение для null-значения имени
/// </summary>
public class NameNullValueException()
    : ArgumentNullException("Name cannot be null");
