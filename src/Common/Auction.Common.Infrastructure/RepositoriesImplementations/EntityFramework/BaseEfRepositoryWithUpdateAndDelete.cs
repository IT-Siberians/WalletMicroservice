using Auction.Common.Domain.Entities;
using Auction.Common.Domain.RepositoriesAbstractions.Base;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Auction.Common.Infrastructure.RepositoriesImplementations.EntityFramework;

public class BaseEfRepositoryWithUpdateAndDelete<TDbContext, TEntity, TKey>(TDbContext dbContext)
    : BaseEfRepositoryWithUpdate<TDbContext, TEntity, TKey>(dbContext),
    IBaseRepositoryWithUpdateAndDelete<TEntity, TKey>
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
        entity.MarkAsDeletedSoftly();
        return Update(entity);
    }

    /// <summary>
    /// Удаляет сущность по её уникальному идентификатору
    /// </summary>
    /// <param name="id">Уникальный идентификатор сущности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>true если сущность существует, иначе false</returns>
    public virtual async Task<bool> DeleteByIdAsync(
        TKey id,
        CancellationToken cancellationToken)
    {
        var entity = await GetByIdAsync(id, cancellationToken: cancellationToken);

        if (entity == null)
        {
            return false;
        }

        return Delete(entity);
    }
}
