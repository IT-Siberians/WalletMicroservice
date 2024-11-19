using Auction.Common.Application.L2.Interfaces.Commands;
using Auction.Common.Application.L2.Interfaces.Pages;
using Auction.Common.Application.L2.Interfaces.Repositories.Base;
using Auction.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Infrastructure.Repositories.EntityFramework;

/// <summary>
/// Базовый класс EntityFramework-репозитория.
/// Позволяет получать и добавлять сущности
/// </summary>
/// <typeparam name="TDbContext">Тип DB-контекста</typeparam>
/// <typeparam name="TEntity">Тип сущности</typeparam>
/// <typeparam name="TKey">Тип уникального идентификатора сущности</typeparam>
/// <remarks>
/// Конструктор базового EntityFramework-репозитория
/// </remarks>
/// <param name="dbContext">Значение DB-контекста</param>
/// <exception cref="ArgumentNullException">Для null-значения</exception>
public class BaseEfRepository<TDbContext, TEntity, TKey>(TDbContext dbContext)
    : IBaseRepository<TEntity, TKey>
        where TDbContext : DbContext
        where TEntity : class, IEntity<TKey>
        where TKey : struct, IEquatable<TKey>
{
    private bool _isDisposed;

    protected readonly TDbContext DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    protected readonly DbSet<TEntity> DbSet = dbContext?.Set<TEntity>() ?? throw new ArgumentNullException(nameof(dbContext));

    /// <summary>
    /// Возвращает сущности удовлетворяющие фильтру
    /// </summary>
    /// <param name="filters">Фильтры</param>
    /// <param name="orderKeySelector">Выбор ключа сортировки</param>
    /// <param name="pageQuery">Параметры возвращаемой страницы данных</param>
    /// <param name="includeProperties">Загружаемые свойства</param>
    /// <param name="useTracking">Отслеживать возвращаемые сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Страница сущностей</returns>
    public virtual async Task<IPageOf<TEntity>> GetAsync<TOrderKey>(
        Expression<Func<TEntity, bool>>[]? filters = null,
        Expression<Func<TEntity, TOrderKey>>? orderKeySelector = null,
        PageQuery? pageQuery = null,
        string? includeProperties = null,
        bool useTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = useTracking
            ? DbSet
            : DbSet.AsNoTracking();

        if (filters is not null)
        {
            foreach (var filter in filters)
            {
                query = query.Where(filter);
            }
        }

        if (includeProperties is not null)
        {
            var propertiesArray = includeProperties.Split(", ");

            foreach (var includeProperty in propertiesArray)
            {
                query = query.Include(includeProperty);
            }
        }

        if (orderKeySelector is not null)
        {
            query = query.OrderBy(orderKeySelector);
        }

        var itemsCount = query.Count();

        if (pageQuery is not null)
        {
            query = query
                .Skip((pageQuery.Number - 1) * pageQuery.ItemsCount)
                .Take(pageQuery.ItemsCount);
        }

        var items = await query.ToListAsync(cancellationToken);

        var pageNumber = pageQuery?.Number ?? 1;
        var pageItemsCount = pageQuery?.ItemsCount ?? itemsCount;
        var pagesCount = pageQuery is null ? 1 : (int)Math.Ceiling((double)itemsCount / pageQuery.ItemsCount);

        var resultPage = new PageOf<TEntity>(
                                itemsCount,
                                pageItemsCount,
                                pagesCount,
                                pageNumber,
                                items);

        return resultPage;
    }

    /// <summary>
    /// Возвращает сущность по её уникальному идентификатору
    /// </summary>
    /// <param name="id">Значение уникального идентификатора</param>
    /// <param name="includeProperties">Загружаемые свойства</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Найденная сущность или null</returns>
    public virtual Task<TEntity?> GetByIdAsync(
        TKey id,
        string? includeProperties = null,
        bool useTracking = true,
        CancellationToken cancellationToken = default)
    {
        IQueryable<TEntity> query = useTracking
            ? DbSet
            : DbSet.AsNoTracking();

        if (includeProperties is not null)
        {
            var propertiesArray = includeProperties.Split(", ");

            foreach (var includeProperty in propertiesArray)
            {
                query = query.Include(includeProperty);
            }
        }

        return query.FirstOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }

    /// <summary>
    /// Добавляет новую сущность
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>true если сущность добавлена, иначе false</returns>
    public virtual async Task<bool> AddAsync(
        TEntity entity,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var entry = await DbSet.AddAsync(entity, cancellationToken);

        return entry.State == EntityState.Added;
    }

    /// <summary>
    /// Сохраняет изменения
    /// </summary>
    public virtual void SaveChanges() => DbContext.SaveChanges();

    /// <summary>
    /// Сохраняет изменения асинхронно
    /// </summary>
    /// <param name="cancellationToken">Токен отмены</param>
    public virtual Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => DbContext.SaveChangesAsync(cancellationToken);

    public virtual void Dispose()
    {
        if (!_isDisposed)
        {
            DbContext.Dispose();

            _isDisposed = true;
        }

        GC.SuppressFinalize(this);
    }
}
