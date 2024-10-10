using System;

namespace Auction.WalletMicroservice.Application.Models.Traiding;

public record PayForLotModel(
    Guid BuyerId,
    Guid SellerId,
    Guid LotId,
    decimal HammerPrice);
