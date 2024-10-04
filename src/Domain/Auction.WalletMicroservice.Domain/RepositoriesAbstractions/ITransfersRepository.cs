﻿using Auction.Common.Domain.RepositoriesAbstractions.Base;
using Auction.WalletMicroservice.Domain.Entities;
using System;

namespace Auction.WalletMicroservice.Domain.RepositoriesAbstractions;

public interface ITransfersRepository
    : IBaseRepository<Transfer, Guid>;
