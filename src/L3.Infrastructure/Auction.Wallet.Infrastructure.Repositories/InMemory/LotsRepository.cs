using Auction.Common.Infrastructure.Repositories.InMemory;
using Auction.Wallet.Application.L2.Interfaces.Repositories;
using Auction.WalletMicroservice.Domain.Entities;
using System;
using System.Collections.Generic;

namespace Auction.Wallet.Infrastructure.Repositories.InMemory;

public class LotsRepository(IList<Lot> entities)
    : BaseMemoryRepository<Lot, Guid>(entities),
    ILotsRepository;
