using Auction.Common.Infrastructure.RepositoriesImplementations.EntityFramework;
using Auction.WalletMicroservice.Domain.Entities;
using Auction.WalletMicroservice.Domain.RepositoriesAbstractions;
using Auction.WalletMicroservice.Infrastructure.EntityFramework;
using System;

namespace Auction.WalletMicroservice.Infrastructure.RepositoriesImplementations.EntityFramework;

public class FreezingsRepository(ApplicationDbContext dbContext)
    : BaseEfRepository<ApplicationDbContext, Freezing, Guid>(dbContext),
    IFreezingsRepository;