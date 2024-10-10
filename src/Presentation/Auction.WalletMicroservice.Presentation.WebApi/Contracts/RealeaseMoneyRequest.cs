using System;

namespace Auction.WalletMicroservice.Presentation.WebApi.Contracts;

public record RealeaseMoneyRequest(
    Guid BuyerId,
    Guid LotId,
    decimal Price);
