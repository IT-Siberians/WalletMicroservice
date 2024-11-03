using System;

namespace Auction.Wallet.Presentation.WebApi.Contracts;

public record PayForLotCommandHttp(
    Guid BuyerId,
    Guid SellerId,
    Guid LotId,
    decimal HammerPrice);
