using System;

namespace Auction.Common.Domain.ValueObjectsExceptions;

/// <summary>
/// Исключение домена когда метод IsValid возвращает false, хотя метод Validate не бросил исключение
/// </summary>
public class ValidationInconsistencyException()
    : Exception("Inconsistency between Validate and IsValid methods");
