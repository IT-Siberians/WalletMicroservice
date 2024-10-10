using System;

namespace Auction.WalletMicroservice.Application.Models.Traiding;

public record RealeaseMoneyModel(
    Guid BuyerId,
    Guid LotId,
    decimal Price);
