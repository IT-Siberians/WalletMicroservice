using Auction.Common.Domain.Entities;
using Auction.Common.Domain.RepositoriesAbstractions.Base;
using Microsoft.EntityFrameworkCore;
using System;

namespace Auction.Common.Infrastructure.RepositoriesImplementations.EntityFramework;

public class BaseEfRepositoryWithUpdate<TDbContext, TEntity, TKey>(TDbContext dbContext)
    : BaseEfRepository<TDbContext, TEntity, TKey>(dbContext),
    IBaseRepositoryWithUpdate<TEntity, TKey>
        where TDbContext : DbContext
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

        var entry = DbContext.Update(entity);

        return entry.State == EntityState.Modified;
    }
}
