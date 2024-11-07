using System;

namespace Auction.Wallet.Presentation.WebApi.Contracts.Owner;

public record WithdrawMoneyFromWalletCommandWeb(
    Guid OwnerId,
    decimal Money);
