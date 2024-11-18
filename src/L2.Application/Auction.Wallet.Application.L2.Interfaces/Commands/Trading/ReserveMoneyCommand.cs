using Auction.Common.Application.L1.Models;
using System;

namespace Auction.Wallet.Application.L2.Interfaces.Commands.Trading;

public record ReserveMoneyCommand(
    Guid BuyerId,
    decimal Price,
    LotInfoModel Lot);
