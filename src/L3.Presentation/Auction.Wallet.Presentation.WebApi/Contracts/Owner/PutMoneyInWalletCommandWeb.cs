using System;

namespace Auction.Wallet.Presentation.WebApi.Contracts.Owner;

public record PutMoneyInWalletCommandWeb(
    Guid OwnerId,
    decimal Money);
