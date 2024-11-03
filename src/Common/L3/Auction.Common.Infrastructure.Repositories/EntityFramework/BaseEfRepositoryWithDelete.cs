using Auction.Common.Application.L2.Interfaces.Repositories.Base;
using Auction.Common.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Infrastructure.Repositories.EntityFramework;

public class BaseEfRepositoryWithDelete<TDbContext, TEntity, TKey>(TDbContext dbContext)
    : BaseEfRepository<TDbContext, TEntity, TKey>(dbContext),
    IBaseRepositoryWithDelete<TEntity, TKey>
        where TDbContext : DbContext
        where TEntity : class, IEntity<TKey>, IDeletableSoftly
        where TKey : struct, IEquatable<TKey>
{
    /// <summary>
    /// Удаляет сущность
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <returns>true если сущность существует, иначе false</returns>
    public virtual bool Delete(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        entity.MarkAsDeletedSoftly();
        var entry = DbSet.Update(entity);

        return entry.State == EntityState.Modified;
    }

    /// <summary>
    /// Удаляет сущность по её уникальному идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>true если сущность существует, иначе false</returns>
    public virtual async Task<bool> DeleteByIdAsync(
        TKey id,
        CancellationToken cancellationToken = default)
    {
        var entity = await GetByIdAsync(id, cancellationToken: cancellationToken);

        if (entity is null)
        {
            return false;
        }

        return Delete(entity);
    }
}