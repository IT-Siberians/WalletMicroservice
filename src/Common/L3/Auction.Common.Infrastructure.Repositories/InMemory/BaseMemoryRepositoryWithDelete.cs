using Auction.Common.Application.L2.Interfaces.Repositories.Base;
using Auction.Common.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Infrastructure.Repositories.InMemory;

public class BaseMemoryRepositoryWithDelete<TEntity, TKey>(IList<TEntity> entities)
    : BaseMemoryRepository<TEntity, TKey>(entities),
    IBaseRepositoryWithDelete<TEntity, TKey>
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
        entity.MarkAsDeletedSoftly();
        return true;
    }

    /// <summary>
    /// Удаляет сущность по её уникальному идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор сущности</param>
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
