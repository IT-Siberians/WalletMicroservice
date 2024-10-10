namespace Auction.WalletMicroservice.Application.Models.Owner;

public record BalanceModel(
    decimal AllMoney,
    decimal FrozenMoney,
    decimal FreeMoney);
