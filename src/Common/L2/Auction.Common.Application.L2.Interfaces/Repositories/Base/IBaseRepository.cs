using Auction.Common.Application.L2.Interfaces.Repositories.Partial;
using Auction.Common.Domain.Entities;
using System;

namespace Auction.Common.Application.L2.Interfaces.Repositories.Base;

public interface IBaseRepository<TEntity, TKey>
    : IDisposable,
    ISaveableRepository,
    IGetableRepository<TEntity, TKey>,
    IAddableRepository<TEntity>
        where TEntity : class, IEntity<TKey>
        where TKey : struct, IEquatable<TKey>;
