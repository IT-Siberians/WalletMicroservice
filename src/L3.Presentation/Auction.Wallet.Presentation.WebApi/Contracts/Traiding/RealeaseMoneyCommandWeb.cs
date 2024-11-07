using System;

namespace Auction.Wallet.Presentation.WebApi.Contracts.Traiding;

public record RealeaseMoneyCommandWeb(
    Guid BuyerId,
    Guid LotId,
    decimal Price);
