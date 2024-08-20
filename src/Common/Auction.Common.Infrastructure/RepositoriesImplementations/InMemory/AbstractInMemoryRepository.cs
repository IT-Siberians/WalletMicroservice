using Auction.Common.Domain.Entities;
using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.RepositoriesAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction.Common.Infrastructure.RepositoriesImplementations.InMemory;

/// <summary>
/// Базовый класс репозитория, хранящего данные в памяти
/// </summary>
/// <typeparam name="TEntity">Тип сущности</typeparam>
/// <typeparam name="TKey">Тип уникального идентификатора сущности</typeparam>
public abstract class AbstractInMemoryRepository<TEntity, TKey>
    : IRepository<TEntity, TKey>
    where TEntity : class, IEntity<TKey>
    where TKey : notnull, IEquatable<TKey>
{
    protected readonly IList<TEntity> _entities;

    /// <summary>
    /// Конструктор базового репозитория, хранящего данные в памяти
    /// </summary>
    /// <param name="entities">Перечисление сущностей</param>
    /// <exception cref="ArgumentNullValueException">Для null-значения</exception>
    protected AbstractInMemoryRepository(IEnumerable<TEntity> entities)
    {
        _entities = entities
            ?.ToList()
            ?? throw new ArgumentNullValueException(nameof(entities));
    }

    /// <summary>
    /// Возвращает все сущности
    /// </summary>
    /// <returns>Перечисление сущностей</returns>
    public virtual Task<IEnumerable<TEntity>> GetAllAsync()
        => Task.FromResult(_entities.AsEnumerable());

    /// <summary>
    /// Возвращает сущность по её уникальному идентификатору
    /// </summary>
    /// <param name="id">Значение уникального идентификатора</param>
    /// <returns>Найденная сущность или null</returns>
    public virtual Task<TEntity?> GetByIdAsync(TKey id)
        => Task.FromResult(_entities.FirstOrDefault(x => x.Id.Equals(id)));

    /// <summary>
    /// Добавляет новую сущность
    /// </summary>
    /// <param name="entity">Сущность</param>
    public virtual Task AddAsync(TEntity entity)
    {
        CheckEntity(entity);

        _entities.Add(entity);
        return Task.CompletedTask;
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
    public virtual Task<bool> UpdateAsync(TEntity entity)
    {
        CheckEntity(entity);

        var existingEntity = _entities.FirstOrDefault(e => e.Id.Equals(entity.Id));

        if (existingEntity == null)
        {
            return Task.FromResult(false);
        }

        var index = _entities.IndexOf(existingEntity);
        _entities[index] = entity;

        return Task.FromResult(true);
    }

    /// <summary>
    /// Удаляет сущность
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <returns>true если сущность существует и ее удалось удалить, иначе false</returns>
    public virtual Task<bool> DeleteAsync(TEntity entity)
    {
        CheckEntity(entity);

        if (_entities.Remove(entity))
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
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
