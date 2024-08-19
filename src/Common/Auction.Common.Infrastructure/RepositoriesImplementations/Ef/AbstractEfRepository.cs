using Auction.Common.Domain.Entities;
using Auction.Common.Domain.Exceptions;
using Auction.Common.Domain.RepositoriesAbstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction.Common.Infrastructure.RepositoriesImplementations.Ef;

public class AbstractEfRepository<TDbContext, TEntity, TKey>
    : IRepository<TEntity, TKey>
    where TDbContext : DbContext
    where TEntity : class, IEntity<TKey>
    where TKey : notnull, IEquatable<TKey>
{
    protected readonly TDbContext _dbContext;

    protected AbstractEfRepository(TDbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullValueException(nameof(dbContext));
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        => (await _dbContext.Set<TEntity>().ToListAsync()).AsEnumerable();

    public virtual Task<TEntity?> GetByIdAsync(TKey id)
        => _dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id.Equals(id));

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
