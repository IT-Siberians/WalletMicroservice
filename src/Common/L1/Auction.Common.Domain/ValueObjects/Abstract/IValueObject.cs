namespace Auction.Common.Domain.ValueObjects.Abstract;

/// <summary>
/// Интерфейс объекта значения
/// </summary>
/// <typeparam name="T">Тип значения</typeparam>
public interface IValueObject<T>
{
    /// <summary>
    /// Значение
    /// </summary>
    public T Value { get; }
}
