using System;

namespace Auction.Wallet.Presentation.WebApi.Contracts;

public record ReserveMoneyCommandHttp(
    Guid BuyerId,
    decimal Price,
    LotInfoModelHttp Lot);
