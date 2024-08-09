using Auction.Common.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Auction.Common.Domain.RepositoriesAbstractions;

public interface IRepository<TEntity, in TKey>
    where TEntity : IEntity<TKey>
    where TKey : notnull, IEquatable<TKey>
{
    Task<IEnumerable<TEntity>> GetAllAsync();
    Task<TEntity?> GetByIdAsync(TKey id);
    Task AddAsync(TEntity entity);
    Task<bool> UpdateAsync(TEntity entity);
    Task<bool> DeleteAsync(TEntity entity);
    Task<bool> DeleteAsync(TKey id);
}
