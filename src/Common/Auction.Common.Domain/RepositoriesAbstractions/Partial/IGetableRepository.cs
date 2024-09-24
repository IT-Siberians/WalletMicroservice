using Auction.Common.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Domain.RepositoriesAbstractions.Partial;

/// <summary>
/// Базовый интерфейс репозитория
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
/// <typeparam name="TKey">Тип уникального идетификатора сущности</typeparam>
public interface IGetableRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : struct, IEquatable<TKey>
{
    /// <summary>
    /// Возвращает сущности удовлетворяющие фильтру
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <param name="orderKeySelector">Выбор ключа сортировки</param>
    /// <param name="includeProperties">Загружаемые свойства</param>
    /// <param name="useTracking">Отслеживать возвращаемые сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Перечисление сущностей</returns>
    Task<IEnumerable<TEntity>> GetAsync<TOrderKey>(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, TOrderKey>>? orderKeySelector = null,
        string[]? includeProperties = null,
        bool useTracking = true,
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Возвращает сущность по её уникальному идентификатору
    /// </summary>
    /// <param name="id">Значение уникального идентификатора</param>
    /// <param name="includeProperties">Загружаемые свойства</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Найденная сущность или null</returns>
    Task<TEntity?> GetByIdAsync(
        TKey id,
        string[]? includeProperties = null,
        CancellationToken cancellationToken = default);
}
