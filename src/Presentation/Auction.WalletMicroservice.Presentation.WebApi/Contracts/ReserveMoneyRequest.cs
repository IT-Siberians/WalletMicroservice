using System;

namespace Auction.WalletMicroservice.Presentation.WebApi.Contracts;

public record ReserveMoneyRequest(
    Guid BuyerId,
    decimal Price,
    LotInfoDto Lot);
