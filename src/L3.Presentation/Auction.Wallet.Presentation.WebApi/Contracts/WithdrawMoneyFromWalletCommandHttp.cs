using System;

namespace Auction.Wallet.Presentation.WebApi.Contracts;

public record WithdrawMoneyFromWalletCommandHttp(
    Guid OwnerId,
    decimal Money);
