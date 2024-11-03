using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.Common.Application.L2.Interfaces.Pages;
using Auction.Common.Application.L2.Interfaces.Repositories.Base;
using Auction.Common.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Infrastructure.Repositories.InMemory;

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
    /// <param name="pageQuery">Параметры возвращаемой страницы данных</param>
    /// <returns>Перечисление сущностей</returns>
    public virtual Task<IPageOf<TEntity>> GetAsync<TOrderKey>(
        Expression<Func<TEntity, bool>>? filter = null,
        Expression<Func<TEntity, TOrderKey>>? orderKeySelector = null,
        PageQuery? pageQuery = null,
        string? includeProperties = null,
        bool useTracking = true,
        CancellationToken cancellationToken = default)
    {
        var entities = Entities;

        if (filter is not null)
        {
            entities = entities
                .Where(filter.Compile())
                .ToList();
        }

        if (orderKeySelector is not null)
        {
            entities = entities
                .OrderBy(orderKeySelector.Compile())
                .ToList();
        }

        var itemsCount = entities.Count;

        if (pageQuery is not null)
        {
            entities = entities
                .Skip((pageQuery.Number - 1) * pageQuery.ItemsCount)
                .Take(pageQuery.ItemsCount)
                .ToList();
        }

        var pageNumber = pageQuery?.Number ?? 1;
        var pageItemsCount = pageQuery?.ItemsCount ?? itemsCount;
        var pagesCount = pageQuery is null ? 1 : (int)Math.Ceiling((double)itemsCount / pageQuery.ItemsCount);

        var resultPage = new PageOf<TEntity>(
                                itemsCount,
                                pageItemsCount,
                                pagesCount,
                                pageNumber,
                                entities);

        return Task.FromResult<IPageOf<TEntity>>(resultPage);
    }

    /// <summary>
    /// Возвращает сущность по её уникальному идентификатору
    /// </summary>
    /// <param name="id">Значение уникального идентификатора</param>
    /// <returns>Найденная сущность или null</returns>
    public virtual Task<TEntity?> GetByIdAsync(
        TKey id,
        string? includeProperties = null,
        bool useTracking = true,
        CancellationToken cancellationToken = default)
            => Task.FromResult(Entities.FirstOrDefault(x => x.Id.Equals(id)));

    /// <summary>
    /// Добавляет новую сущность
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <returns>true если сущность добавлена, иначе false</returns>
    public virtual Task<bool> AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
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
    public virtual Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => Task.CompletedTask;

    public virtual void Dispose()
    {
        GC.SuppressFinalize(this);
    }
}
