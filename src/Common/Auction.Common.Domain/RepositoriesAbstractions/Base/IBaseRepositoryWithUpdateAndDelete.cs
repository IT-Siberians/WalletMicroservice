using Auction.Common.Domain.Entities;
using Auction.Common.Domain.RepositoriesAbstractions.Partial;
using System;

namespace Auction.Common.Domain.RepositoriesAbstractions.Base;

public interface IBaseRepositoryWithUpdateAndDelete<TEntity, TKey>
    : IBaseRepositoryWithUpdate<TEntity, TKey>,
    IDeletableRepository<TEntity, TKey>
        where TEntity : class, IEntity<TKey>
        where TKey : struct, IEquatable<TKey>;
