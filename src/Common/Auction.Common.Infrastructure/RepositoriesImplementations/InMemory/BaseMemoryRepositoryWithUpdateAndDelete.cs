using Auction.Common.Domain.Entities;
using Auction.Common.Domain.RepositoriesAbstractions.Base;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Infrastructure.RepositoriesImplementations.InMemory;

public class BaseMemoryRepositoryWithUpdateAndDelete<TEntity, TKey>(IList<TEntity> entities)
    : BaseMemoryRepositoryWithUpdate<TEntity, TKey>(entities),
    IBaseRepositoryWithUpdateAndDelete<TEntity, TKey>
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
        return Update(entity);
    }

    /// <summary>
    /// Удаляет сущность по её уникальному идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор сущности</param>
    /// <returns>true если сущность существует, иначе false</returns>
    public virtual async Task<bool> DeleteByIdAsync(
        TKey id,
        CancellationToken _ = default)
    {
        var entity = await GetByIdAsync(id);

        if (entity == null)
        {
            return false;
        }

        return Delete(entity);
    }
}
