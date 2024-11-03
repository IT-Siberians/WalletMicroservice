using Auction.Common.Application.L1.Models;
using System;

namespace Auction.Wallet.Application.L1.Models.Owners;

public record TransactionModel(
    decimal? TransferMoney,
    decimal? FreezingMoney,
    DateTime DateTime,
    TransactionType Type,
    LotInfoModel? Lot)
{
    public string TypeName => Enum.GetName(Type) ?? Type.ToString();

    public BalanceModel? Balance { get; set; }
}
