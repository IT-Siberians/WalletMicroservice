using Auction.Common.Domain.Entities;
using Auction.Common.Infrastructure.RepositoriesImplementations.Ef;
using Auction.WalletMicroservice.Infrastructure.EntityFramework;
using System;

namespace Auction.WalletMicroservice.Infrastructure.RepositoriesImplementations.Ef;

public class EfRepository<TEntity>(ApplicationDbContext dbContext)
    : AbstractEfRepository<ApplicationDbContext, TEntity, Guid>(dbContext)
    where TEntity : class, IEntity<Guid>;
