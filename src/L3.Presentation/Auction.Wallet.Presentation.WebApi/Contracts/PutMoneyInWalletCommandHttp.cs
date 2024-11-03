using System;

namespace Auction.Wallet.Presentation.WebApi.Contracts;

public record PutMoneyInWalletCommandHttp(
    Guid OwnerId,
    decimal Money);
