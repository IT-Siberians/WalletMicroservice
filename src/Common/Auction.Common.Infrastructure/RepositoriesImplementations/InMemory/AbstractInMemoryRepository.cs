using Auction.Common.Domain.Entities;
using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.RepositoriesAbstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction.Common.Infrastructure.RepositoriesImplementations.InMemory;

public abstract class AbstractInMemoryRepository<TEntity, TKey>
    : IRepository<TEntity, TKey>
    where TEntity : IEntity<TKey>
    where TKey : notnull, IEquatable<TKey>
{
    protected readonly IList<TEntity> _entities;

    protected AbstractInMemoryRepository(IEnumerable<TEntity> entities)
    {
        _entities = entities
            ?.ToList()
            ?? throw new ArgumentNullValueException(nameof(entities));
    }

    public virtual Task<IEnumerable<TEntity>> GetAllAsync()
        => Task.FromResult(_entities.AsEnumerable());

    public virtual Task<TEntity?> GetByIdAsync(TKey id)
        => Task.FromResult(_entities.FirstOrDefault(x => x.Id.Equals(id)));

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

    public virtual Task<bool> DeleteAsync(TEntity entity)
    {
        CheckEntity(entity);

        if (_entities.Remove(entity))
        {
            return Task.FromResult(true);
        }

        return Task.FromResult(false);
    }

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
