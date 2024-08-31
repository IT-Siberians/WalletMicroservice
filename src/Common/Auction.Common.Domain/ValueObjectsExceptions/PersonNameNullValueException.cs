using System;

namespace Auction.Common.Domain.ValueObjectsExceptions;

/// <summary>
/// Доменное исключение для null-значения имени
/// </summary>
public class PersonNameNullValueException()
    : ArgumentNullException("Name cannot be null");
