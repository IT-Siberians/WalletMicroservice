using System;

namespace Auction.Wallet.Presentation.WebApi.Contracts.Traiding;

public record PayForLotCommandWeb(
    Guid BuyerId,
    Guid SellerId,
    Guid LotId,
    decimal HammerPrice);
