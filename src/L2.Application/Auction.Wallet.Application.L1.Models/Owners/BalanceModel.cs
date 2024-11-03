namespace Auction.Wallet.Application.L1.Models.Owners;

public record BalanceModel(
    decimal AllMoney,
    decimal FrozenMoney,
    decimal FreeMoney);
