using System;

namespace Auction.Common.Domain.ValueObjectsExceptions;

/// <summary>
/// Исключение домена для случая, когда метод IsValid возвращает false, хотя метод Validate не бросил исключение
/// </summary>
internal class ValidationInconsistencyException()
    : Exception("Inconsistency between Validate and IsValid methods");
