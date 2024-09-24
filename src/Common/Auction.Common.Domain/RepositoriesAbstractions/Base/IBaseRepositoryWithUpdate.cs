using Auction.Common.Domain.Entities;
using Auction.Common.Domain.RepositoriesAbstractions.Partial;
using System;

namespace Auction.Common.Domain.RepositoriesAbstractions.Base;

public interface IBaseRepositoryWithUpdate<TEntity, TKey>
    : IBaseRepository<TEntity, TKey>,
    IUpdatableRepository<TEntity>
        where TEntity : class, IEntity<TKey>
        where TKey : struct, IEquatable<TKey>;
