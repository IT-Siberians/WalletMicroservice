using System;

namespace Auction.Wallet.Application.L2.Interfaces.Commands.Trading;

public record RealeaseMoneyCommand(
    Guid BuyerId,
    Guid LotId,
    decimal Price);
