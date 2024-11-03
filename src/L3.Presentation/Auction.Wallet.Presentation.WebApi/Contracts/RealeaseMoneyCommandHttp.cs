using System;

namespace Auction.Wallet.Presentation.WebApi.Contracts;

public record RealeaseMoneyCommandHttp(
    Guid BuyerId,
    Guid LotId,
    decimal Price);
