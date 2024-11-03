using System;

namespace Auction.Common.Domain.EntitiesExceptions;

/// <summary>
/// Исключение домена для несовместимых значений аргумента
/// </summary>
/// <typeparam name="T1">Тип 1го аргумента</typeparam>
/// <typeparam name="T2">Тип 2го аргумента</typeparam>
/// <param name="argument1Name">Имя 1го аргумента</param>
/// <param name="argument1Value">Значение 1го аргумента</param>
/// <param name="argument2Name">Имя 2го аргумента</param>
/// <param name="argument2Value">Значение 2го аргумента</param>
/// <param name="explanationMessage">Пояснение</param>
public class IncompatibleArgumentsValuesException<T1, T2>(
    string argument1Name,
    T1 argument1Value,
    string argument2Name,
    T2 argument2Value,
    string explanationMessage) : ArgumentException($"Arguments \"{argument1Name}\" (value \"{argument1Value}\") and \"{argument2Name}\" (value \"{argument2Value}\") have incompatible values. {explanationMessage}")
{
    /// <summary>
    /// Значение 1го аргумента
    /// </summary>
    public T1 Argument1 { get; } = argument1Value;

    /// <summary>
    /// Значение 2го аргумента
    /// </summary>
    public T2 Argument2 { get; } = argument2Value;
}
