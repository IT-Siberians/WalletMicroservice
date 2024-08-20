using Auction.Common.Domain.Entities;
using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.RepositoriesAbstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction.Common.Infrastructure.RepositoriesImplementations.Ef;

/// <summary>
/// Базовый класс EntityFramework-репозитория
/// </summary>
/// <typeparam name="TDbContext">Тип DB-контекста</typeparam>
/// <typeparam name="TEntity">Тип сущности</typeparam>
/// <typeparam name="TKey">Тип уникального идентификатора сущности</typeparam>
public class AbstractEfRepository<TDbContext, TEntity, TKey>
    : IRepository<TEntity, TKey>
    where TDbContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : notnull, IEquatable<TKey>
{
    protected readonly TDbContext _dbContext;

    /// <summary>
    /// Конструктор базового EntityFramework-репозитория
    /// </summary>
    /// <param name="dbContext">Значение DB-контекста</param>
    /// <exception cref="ArgumentNullValueException">Для null-значения</exception>
    protected AbstractEfRepository(TDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullValueException(nameof(dbContext));
    }

    /// <summary>
    /// Возвращает все сущности
    /// </summary>
    /// <returns>Перечисление сущностей</returns>
    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        => (await _dbContext.Set<TEntity>().ToListAsync()).AsEnumerable();

    /// <summary>
    /// Возвращает сущность по её уникальному идентификатору
    /// </summary>
    /// <param name="id">Значение уникального идентификатора</param>
    /// <returns>Найденная сущность или null</returns>
    public virtual Task<TEntity?> GetByIdAsync(TKey id)
        => _dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id.Equals(id));

    /// <summary>
    /// Добавляет новую сущность
    /// </summary>
    /// <param name="entity">Сущность</param>
    public virtual async Task AddAsync(TEntity entity)
    {
        CheckEntity(entity);

        _dbContext.Add(entity);
        await _dbContext.SaveChangesAsync();
    }

    protected void CheckEntity(TEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));
    }

    /// <summary>
    /// Обновляет состояние сущности
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <returns>true если сущность существует и ее удалось обновить, иначе false</returns>
    public virtual async Task<bool> UpdateAsync(TEntity entity)
    {
        CheckEntity(entity);

        var existingEntity = await _dbContext
            .Set<TEntity>()
            .FirstOrDefaultAsync(e => e.Id.Equals(entity.Id));

        if (existingEntity == null)
        {
            return false;
        }

        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Удаляет сущность
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <returns>true если сущность существует и ее удалось удалить, иначе false</returns>
    public virtual async Task<bool> DeleteAsync(TEntity entity)
    {
        CheckEntity(entity);

        var existingEntity = await _dbContext
            .Set<TEntity>()
            .FirstOrDefaultAsync(e => e.Id.Equals(entity.Id));

        if (existingEntity == null)
        {
            return false;
        }

        _dbContext.Remove(entity);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Удаляет сущность по её уникальному идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор сущности</param>
    /// <returns>true если сущность существует и ее удалось удалить, иначе false</returns>
    public virtual async Task<bool> DeleteAsync(TKey id)
    {
        var existingEntity = await GetByIdAsync(id);

        if (existingEntity == null)
        {
            return false;
        }

        return await DeleteAsync(existingEntity);
    }
}
