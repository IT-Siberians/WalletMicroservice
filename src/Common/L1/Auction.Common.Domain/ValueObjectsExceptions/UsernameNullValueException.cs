using System;

namespace Auction.Common.Domain.ValueObjectsExceptions;

/// <summary>
/// Доменное исключение для null-значения имени
/// </summary>
internal class UsernameNullValueException()
    : ArgumentNullException("Name cannot be null");
