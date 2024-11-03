using System;

namespace Auction.Wallet.Application.L2.Interfaces.Commands.Traiding;

public record RealeaseMoneyCommand(
    Guid BuyerId,
    Guid LotId,
    decimal Price);
