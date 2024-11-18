using System;

namespace Auction.Wallet.Application.L2.Interfaces.Commands.Trading;

public record PayForLotCommand(
    Guid BuyerId,
    Guid SellerId,
    Guid LotId,
    decimal HammerPrice);
