using System;

namespace Auction.Wallet.Presentation.WebApi.Contracts.Trading;

public record PayForLotCommandWeb(
    Guid BuyerId,
    Guid SellerId,
    Guid LotId,
    decimal HammerPrice);
