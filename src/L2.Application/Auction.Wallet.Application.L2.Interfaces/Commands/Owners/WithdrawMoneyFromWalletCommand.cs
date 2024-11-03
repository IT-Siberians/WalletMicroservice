using System;

namespace Auction.Wallet.Application.L2.Interfaces.Commands.Owners;

public record WithdrawMoneyFromWalletCommand(
    Guid OwnerId,
    decimal Money);
