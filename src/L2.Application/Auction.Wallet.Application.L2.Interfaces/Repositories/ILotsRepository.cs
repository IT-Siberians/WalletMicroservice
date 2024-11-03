using Auction.Common.Application.L2.Interfaces.Repositories.Base;
using Auction.WalletMicroservice.Domain.Entities;
using System;

namespace Auction.Wallet.Application.L2.Interfaces.Repositories;

public interface ILotsRepository
    : IBaseRepository<Lot, Guid>;
