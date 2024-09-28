using Auction.Common.Domain.Entities;
using Auction.Common.Domain.RepositoriesAbstractions.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Infrastructure.RepositoriesImplementations.InMemory;

/// <summary>
/// Базовый класс EntityFramework-репозитория.
/// Позволяет получать и добавлять сущности
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
/// <typeparam name="TKey">Тип уникального идентификатора сущности</typeparam>
/// <remarks>
/// Конструктор базового репозитория, хранящего данные в памяти
/// </remarks>
/// <param name="entities">Перечисление сущностей</param>
/// <exception cref="ArgumentNullException">Для null-значения</exception>
public class BaseMemoryRepository<TEntity, TKey>(IList<TEntity> entities)
    : IBaseRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : struct, IEquatable<TKey>
{
    protected readonly IList<TEntity> Entities = entities ?? throw new ArgumentNullException(nameof(entities));

    /// <summary>
    /// Возвращает сущности удовлетворяющие фильтру
    /// </summary>
    /// <param name="filter">Фильтр</param>
    /// <param name="orderKeySelector">Выбор ключа сортировки</param>
    /// <returns>Перечисление сущностей</returns>
    public virtual Task<IEnumerable<TEntity>> GetAsync<TOrderKey>(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, TOrderKey>>? orderKeySelector = null,
        string[]? _ = null,
        bool __ = true,
        CancellationToken ___ = default)
    {
        var entities = Entities;

        if (filter != null)
        {
            entities = entities
                .Where(filter.Compile())
                .ToList();
        }

        if (orderKeySelector != null)
        {
            entities = entities
                .OrderBy(orderKeySelector.Compile())
                .ToList();
        }

        return Task.FromResult(entities.AsEnumerable());
    }

    /// <summary>
    /// Возвращает сущность по её уникальному идентификатору
    /// </summary>
    /// <param name="id">Значение уникального идентификатора</param>
    /// <returns>Найденная сущность или null</returns>
    public virtual Task<TEntity?> GetByIdAsync(
        TKey id,
        string[]? _ = null,
        CancellationToken __ = default)
            => Task.FromResult(Entities.FirstOrDefault(x => x.Id.Equals(id)));

    /// <summary>
    /// Добавляет новую сущность
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <returns>true если сущность добавлена, иначе false</returns>
    public virtual Task<bool> AddAsync(
        TEntity entity,
        CancellationToken _ = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        Entities.Add(entity);

        return Task.FromResult(true);
    }

    /// <summary>
    /// Сохраняет изменения
    /// </summary>
    public virtual void SaveChanges() { }

    /// <summary>
    /// Сохраняет изменения асинхронно
    /// </summary>
    public virtual Task SaveChangesAsync(CancellationToken _ = default)
        => Task.CompletedTask;

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
