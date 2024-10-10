using System;

namespace Auction.WalletMicroservice.Presentation.WebApi.Contracts;

public record PayForLotRequest(
    Guid BuyerId,
    Guid SellerId,
    Guid LotId,
    decimal HammerPrice);
