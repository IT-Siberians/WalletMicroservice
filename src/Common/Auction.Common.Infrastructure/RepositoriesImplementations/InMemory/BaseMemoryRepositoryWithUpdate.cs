using Auction.Common.Domain.Entities;
using Auction.Common.Domain.RepositoriesAbstractions.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auction.Common.Infrastructure.RepositoriesImplementations.InMemory;

public class BaseMemoryRepositoryWithUpdate<TEntity, TKey>(IList<TEntity> entities)
    : BaseMemoryRepository<TEntity, TKey>(entities),
    IBaseRepositoryWithUpdate<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : struct, IEquatable<TKey>
{
    /// <summary>
    /// Обновляет состояние сущности
    /// </summary>
    /// <param name="entity">Сущность</param>
    /// <returns>true если сущность существует и ее удалось обновить, иначе false</returns>
    public virtual bool Update(TEntity entity)
    {
        ArgumentNullException.ThrowIfNull(entity, nameof(entity));

        var existingEntity = Entities.FirstOrDefault(e => e.Id.Equals(entity.Id));

        if (existingEntity == null)
        {
            return false;
        }

        var index = Entities.IndexOf(existingEntity);
        Entities[index] = entity;

        return true;
    }
}
