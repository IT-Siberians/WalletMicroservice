using Auction.Common.Domain.Entities;
using Auction.Common.Domain.RepositoriesAbstractions.Partial;
using System;

namespace Auction.Common.Domain.RepositoriesAbstractions.Base;

public interface IBaseRepository<TEntity, TKey>
    : IDisposable,
    ISaveableRepository,
    IGetableRepository<TEntity, TKey>,
    IAddableRepository<TEntity>
        where TEntity : class, IEntity<TKey>
        where TKey : struct, IEquatable<TKey>;
