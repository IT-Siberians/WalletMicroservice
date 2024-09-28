﻿using Auction.Common.Infrastructure.RepositoriesImplementations.InMemory;
using Auction.WalletMicroservice.Domain.Entities;
using Auction.WalletMicroservice.Domain.RepositoriesAbstractions;
using System;
using System.Collections.Generic;

namespace Auction.WalletMicroservice.Infrastructure.RepositoriesImplementations.InMemory;

public class TransfersRepository(IList<Transfer> entities)
    : BaseMemoryRepository<Transfer, Guid>(entities),
    ITransfersRepository;
