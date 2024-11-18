using System;

namespace Auction.Wallet.Presentation.WebApi.Contracts.Trading;

public record RealeaseMoneyCommandWeb(
    Guid BuyerId,
    Guid LotId,
    decimal Price);
