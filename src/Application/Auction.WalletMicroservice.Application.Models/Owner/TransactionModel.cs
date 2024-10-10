using Auction.Common.Application.Models.Common;
using System;

namespace Auction.WalletMicroservice.Application.Models.Owner;

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
