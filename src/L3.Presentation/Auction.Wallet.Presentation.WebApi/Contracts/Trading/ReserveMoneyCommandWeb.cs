using Auction.Wallet.Presentation.WebApi.Contracts.Owner;
using System;

namespace Auction.Wallet.Presentation.WebApi.Contracts.Trading;

public record ReserveMoneyCommandWeb(
    Guid BuyerId,
    decimal Price,
    LotInfoModelWeb Lot);
